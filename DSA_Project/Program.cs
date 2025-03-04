using System;
using System.Collections.Generic;

class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }

    public MenuItem(int id, string name, double price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}

class AVLNode
{
    public double Price;
    public string ItemName;
    public AVLNode Left, Right;
    public int Height;

    public AVLNode(double price, string itemName)
    {
        Price = price;
        ItemName = itemName;
        Left = Right = null;
        Height = 1;
    }
}

class AVLTree
{
    public AVLNode Root;

    private int Height(AVLNode node)
    {
        return node == null ? 0 : node.Height;
    }

    private int GetBalance(AVLNode node)
    {
        return node == null ? 0 : Height(node.Left) - Height(node.Right);
    }

    private AVLNode RotateRight(AVLNode y)
    {
        AVLNode x = y.Left;
        AVLNode T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

        return x;
    }

    private AVLNode RotateLeft(AVLNode x)
    {
        AVLNode y = x.Right;
        AVLNode T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

        return y;
    }

    public AVLNode Insert(AVLNode node, double price, string itemName)
    {
        if (node == null)
            return new AVLNode(price, itemName);

        if (price < node.Price)
            node.Left = Insert(node.Left, price, itemName);
        else if (price > node.Price)
            node.Right = Insert(node.Right, price, itemName);
        else
            return node; // No duplicates allowed

        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

        int balance = GetBalance(node);

        // Left Heavy
        if (balance > 1 && price < node.Left.Price)
            return RotateRight(node);

        // Right Heavy
        if (balance < -1 && price > node.Right.Price)
            return RotateLeft(node);

        // Left-Right Case
        if (balance > 1 && price > node.Left.Price)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        // Right-Left Case
        if (balance < -1 && price < node.Right.Price)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    public string Search(AVLNode node, double price)
    {
        if (node == null)
            return "Item not found.";

        if (price == node.Price)
            return $"Found: {node.ItemName} - Rs.{node.Price}";

        if (price < node.Price)
            return Search(node.Left, price);
        else
            return Search(node.Right, price);
    }
}

class Order
{
    public int TableNumber { get; set; }
    public LinkedList<OrderItem> Items { get; set; } // Using LinkedList for items
    public string Status { get; set; }

    public Order(int tableNumber, LinkedList<OrderItem> items)
    {
        TableNumber = tableNumber;
        Items = items;
        Status = "Pending";
    }
}

class OrderItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public OrderItem(string name, int quantity, double price)
    {
        Name = name;
        Quantity = quantity;
        Price = price;
    }
}

class RestaurantManagementSystem
{
    static LinkedList<MenuItem> menu = new LinkedList<MenuItem>(); // Using LinkedList for menu
    static Queue<Order> orderQueue = new Queue<Order>(); // Using Queue for orders
    static Stack<Order> orderHistory = new Stack<Order>(); // Using Stack for order history
    static double totalIncome = 0;
    static AVLTree menuTree = new AVLTree();

    static void Main()
    {
        AddMenu(); // Initialize menu items
        MainMenu();
    }

    // Add Menu Method
    static void AddMenu()
    {
        AddItem(new MenuItem(1, "Burger", 100.00));
        AddItem(new MenuItem(2, "Pizza", 500.00));
        AddItem(new MenuItem(3, "Pasta", 350.00));
    }

    // Add Item to Menu (Using LinkedList)
    static void AddItem(MenuItem item)
    {
        menu.AddLast(item); // Add to LinkedList
        menuTree.Root = menuTree.Insert(menuTree.Root, item.Price, item.Name); // Add to AVL Tree
    }

    // Remove Item from Menu (Using LinkedList)
    static void RemoveItem(int id)
    {
        var node = menu.First;
        while (node != null)
        {
            if (node.Value.Id == id)
            {
                menu.Remove(node);
                Console.WriteLine("Item removed successfully!");
                return;
            }
            node = node.Next;
        }
        Console.WriteLine("Item not found!");
    }

    // Quick Sort Algorithm for Menu Items by Price
    static void QuickSortMenu(MenuItem[] menuArray, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(menuArray, low, high);
            QuickSortMenu(menuArray, low, pivotIndex - 1);
            QuickSortMenu(menuArray, pivotIndex + 1, high);
        }
    }

    // Partition Method for Quick Sort
    static int Partition(MenuItem[] menuArray, int low, int high)
    {
        double pivot = menuArray[high].Price;
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (menuArray[j].Price < pivot)
            {
                i++;
                Swap(menuArray, i, j);
            }
        }
        Swap(menuArray, i + 1, high);
        return i + 1;
    }

    // Swap Method for Quick Sort
    static void Swap(MenuItem[] menuArray, int i, int j)
    {
        MenuItem temp = menuArray[i];
        menuArray[i] = menuArray[j];
        menuArray[j] = temp;
    }

    // Sort Menu Items by Price
    static void SortMenu()
    {
        // Convert LinkedList to Array
        MenuItem[] menuArray = new MenuItem[menu.Count];
        menu.CopyTo(menuArray, 0);

        // Perform Quick Sort
        QuickSortMenu(menuArray, 0, menuArray.Length - 1);

        // Rebuild LinkedList from Sorted Array
        menu = new LinkedList<MenuItem>(menuArray);
    }

    // Main Menu Method
    static void MainMenu()
    {
        while (true)
        {
            Console.WriteLine("--------------------------------------------------------------\n");
            Console.WriteLine("====================WELCOME TO FOOD PALACE====================\n");
            Console.WriteLine("--------------------------------------------------------------\n");
            Console.WriteLine("\t1. Admin Section");
            Console.WriteLine("\t2. Customer Section");
            Console.WriteLine("\t3. Exit\n");
            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AdminSection();
                    break;
                case 2:
                    CustomerSection();
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("Invalid choice! Try again.\n");
                    break;
            }
        }
    }

    // Admin Section Method
    static void AdminSection()
    {
        while (true)
        {
            Console.WriteLine("\n\t================ADMIN SECTION===============");
            Console.WriteLine("\t\t1. Menu Management");
            Console.WriteLine("\t\t2. Order Processing");
            Console.WriteLine("\t\t3. Billing System");
            Console.WriteLine("\t\t4. Back to Main Menu");
            Console.WriteLine("\t\t5. Exit");
            Console.Write("\nEnter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ManageMenu();
                    break;
                case 2:
                    ProcessOrders();
                    break;
                case 3:
                    DisplayBillingSystem();
                    break;
                case 4:
                    return;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice! Try again.");
                    break;
            }
        }
    }

    // Customer Section Method
    static void CustomerSection()
    {
        while (true)
        {
            Console.WriteLine("\n\t==============CUSTOMER SECTION================");
            Console.WriteLine("\t\t1. Place Your Order");
            Console.WriteLine("\t\t2. Show Ordered Food Items");
            Console.WriteLine("\t\t3. Display the Bill");
            Console.WriteLine("\t\t4. Search Item by Price");
            Console.WriteLine("\t\t5. Back to Main Menu");
            Console.WriteLine("\t\t6. Exit");
            Console.Write("\nEnter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    PlaceOrder();
                    break;
                case 2:
                    ShowOrders();
                    break;
                case 3:
                    DisplayBill();
                    break;
                case 4:
                    SearchMenuByPrice();
                    break;
                case 5:
                    return;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice! Try again.\n");
                    break;
            }
        }
    }

    // Manage Menu Method
    static void ManageMenu()
    {
        Console.WriteLine("Menu Management:");
        Console.WriteLine("1. Add Item");
        Console.WriteLine("2. Remove Item");
        Console.WriteLine("3. Show Menu");
        Console.Write("Enter your choice: ");
        int choice = Convert.ToInt32(Console.ReadLine());

        if (choice == 1)
        {
            Console.Write("Enter Item ID: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Item Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Price: ");
            double price = Convert.ToDouble(Console.ReadLine());

            AddItem(new MenuItem(id, name, price));
            Console.WriteLine("Item added successfully!");
        }
        else if (choice == 2)
        {
            Console.Write("Enter Item ID to Remove: ");
            int id = Convert.ToInt32(Console.ReadLine());
            RemoveItem(id);
        }
        else if (choice == 3)
        {
            Console.WriteLine("Updated Menu:");
            DisplayMenu();
        }
    }

    // Display Menu Method
    static void DisplayMenu()
    {
        SortMenu();
        foreach (var item in menu)
        {
            Console.WriteLine($"{item.Id}. {item.Name} - Rs.{item.Price}");
        }
    }

    // Process Orders Method
    static void ProcessOrders()
    {
        while (orderQueue.Count > 0)
        {
            Order order = orderQueue.Dequeue();
            Console.WriteLine($"Processing Order for Table {order.TableNumber}");
            Console.Write("Do you accept this order? (yes/no): ");
            string decision = Console.ReadLine().ToLower();

            if (decision == "yes")
            {
                order.Status = "Approved";
                orderHistory.Push(order);
                double total = 0;

                foreach (var item in order.Items)
                {
                    total += item.Quantity * item.Price;
                }
                totalIncome += total;

                Console.WriteLine("Order accepted and added to history.");
            }
            else if (decision == "no")
            {
                order.Status = "Rejected";
                orderHistory.Push(order);
                Console.WriteLine("Order rejected.");
            }
        }
    }

    // Display Billing System Method
    static void DisplayBillingSystem()
    {
        Console.WriteLine("Order Summaries:");

        foreach (var order in orderHistory)
        {
            Console.WriteLine($"Table {order.TableNumber}:");
            double totalBill = 0;
            foreach (var item in order.Items)
            {
                double cost = item.Quantity * item.Price;
                totalBill += cost;
                Console.WriteLine($"{item.Name} x{item.Quantity} - Rs.{cost}");
            }
            Console.WriteLine($"Total: Rs.{totalBill}");
            Console.WriteLine("---------------------------");
        }
        Console.WriteLine($"Total Income of the Day: Rs.{totalIncome}");
    }

    // Place Order Method
    static void PlaceOrder()
    {
        Console.WriteLine("\n\t------OUR FOOD MENU------");
        Console.WriteLine("Item Id\t\tFood Item\tPrice");
        SortMenu();

        foreach (var item in menu)
        {
            Console.WriteLine($"   {item.Id}\t\t{item.Name}\t\tRs.{item.Price}");
        }

        Console.Write("Enter Table Number: ");
        int tableNumber = Convert.ToInt32(Console.ReadLine());
        LinkedList<OrderItem> items = new LinkedList<OrderItem>();

        while (true)
        {
            Console.Write("Enter Item ID: ");
            int itemId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Quantity: ");
            int qty = Convert.ToInt32(Console.ReadLine());

            var menuItem = menu.FirstOrDefault(item => item.Id == itemId);
            if (menuItem != null)
            {
                items.AddLast(new OrderItem(menuItem.Name, qty, menuItem.Price));
            }
            else
            {
                Console.WriteLine("Invalid item ID!");
            }

            Console.Write("Do you need anything else? (yes/no): ");
            string more = Console.ReadLine().ToLower();
            if (more == "no") break;
        }

        orderQueue.Enqueue(new Order(tableNumber, items));
    }

    // Show Orders Method
    static void ShowOrders()
    {
        Console.WriteLine("Current Orders:");
        foreach (var order in orderQueue)
        {
            Console.WriteLine($"Table {order.TableNumber}:");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"{item.Name} x{item.Quantity}");
            }
            Console.WriteLine("----------------");
        }
    }

    // Display Bill Method
    static void DisplayBill()
    {
        Console.WriteLine("Order Bills:");

        foreach (var order in orderQueue)
        {
            Console.WriteLine($"Table {order.TableNumber}:");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"{item.Name} x{item.Quantity} - Rs.{item.Quantity * item.Price}");
            }
            Console.WriteLine("Status: Waiting for Admin Approval.");
            Console.WriteLine("---------------------------");
        }

        foreach (var order in orderHistory)
        {
            Console.WriteLine($"Table {order.TableNumber}:");
            double totalBill = 0;

            foreach (var item in order.Items)
            {
                double cost = item.Quantity * item.Price;
                totalBill += cost;
                Console.WriteLine($"{item.Name} x{item.Quantity} - Rs.{cost}");
            }

            if (order.Status == "Rejected")
            {
                Console.WriteLine("Status: Sorry, we can't proceed with your order.");
            }
            else if (order.Status == "Approved")
            {
                Console.WriteLine("Status: Admin Approved your order.");
                Console.WriteLine($"Total: Rs.{totalBill}");
            }

            Console.WriteLine("---------------------------");
        }
    }

    // Search Menu by Price Method
    static void SearchMenuByPrice()
    {
        Console.Write("Enter the price to search for: ");
        double price = Convert.ToDouble(Console.ReadLine());

        string result = menuTree.Search(menuTree.Root, price);
        Console.WriteLine(result);
    }
}