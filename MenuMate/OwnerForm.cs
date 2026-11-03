using System.Globalization;

namespace MenuMate
{
    public partial class OwnerForm : Form
    {
        private readonly User currentUser;
        private readonly DataManager dataManager = new DataManager();

        // Brand colors
        private static readonly Color Green = ColorTranslator.FromHtml("#2F6B4F");
        private static readonly Color GreenHover = ColorTranslator.FromHtml("#3D7F5F");
        private static readonly Color GreenSoft = ColorTranslator.FromHtml("#EAF2ED");
        private static readonly Color SelectGreen = ColorTranslator.FromHtml("#DCEBE2");
        private static readonly Color Orange = ColorTranslator.FromHtml("#E8833A");
        private static readonly Color OrangeDark = ColorTranslator.FromHtml("#D4722C");
        private static readonly Color Cream = ColorTranslator.FromHtml("#FAF6EF");
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F7F3EB");
        private static readonly Color Line = ColorTranslator.FromHtml("#E4DED2");
        private static readonly Color DarkText = ColorTranslator.FromHtml("#1F2A24");
        private static readonly Color Muted = ColorTranslator.FromHtml("#6B756F");
        private static readonly Color Red = ColorTranslator.FromHtml("#B3261E");
        private static readonly Color RedSoft = ColorTranslator.FromHtml("#FBEAE8");

        // Controls we need to reach from different methods
        private TabControl tabs = new TabControl();

        // Menu Items tab (A2)
        private DataGridView menuGrid = new DataGridView();
        private Label countLabel = new Label();
        private TextBox nameBox = new TextBox();
        private ComboBox categoryBox = new ComboBox();
        private NumericUpDown priceBox = new NumericUpDown();
        private NumericUpDown prepBox = new NumericUpDown();
        private List<MenuItem> shownMenu = new List<MenuItem>();
        private string? selectedName = null;   // name of the item the owner clicked (null = nothing selected)

        // Kitchen Board tab (A3)
        private Kitchen kitchen = new Kitchen();
        private ListBox receivedList = new ListBox();
        private ListBox preparingList = new ListBox();
        private ListBox readyList = new ListBox();
        private Label receivedTitle = new Label();
        private Label preparingTitle = new Label();
        private Label readyTitle = new Label();
        private System.Windows.Forms.Timer refreshTimer = new System.Windows.Forms.Timer();
        private bool isRefreshing = false;   // true while a refresh is running, so two never overlap

        public OwnerForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            this.Icon = Icon.FromHandle(Properties.Resources.logo.GetHicon());
            BuildUi();
        }

        // One row in a Kitchen Board list. The ListBox shows ToString() and we keep the Order inside.
        private class OrderRow
        {
            public Order Order { get; }

            public OrderRow(Order order)
            {
                Order = order;
            }

            // Example: "#1003 · tom · Takeaway · 2 x Latte, 1 x Muffin"
            public override string ToString()
            {
                List<string> parts = new List<string>();
                foreach (CartItem item in Order.Items)
                {
                    parts.Add(item.Quantity + " x " + item.Item.Name);
                }
                return "#" + Order.OrderNumber + " · " + Order.CustomerName + " · " + Order.Type
                    + " · " + string.Join(", ", parts);
            }
        }

        // ===== Build the window =====
        private void BuildUi()
        {
            this.Text = "MenuMate - Owner";
            this.Font = new Font("Segoe UI", 10f);
            this.Size = new Size(1040, 720);
            this.MinimumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Cream;
            this.ForeColor = DarkText;

            // The Fill control is added first, then the Top header.
            BuildTabs();
            BuildHeader();

            // Windows selects the first row when the window appears, so clear it once the window is shown.
            // The Kitchen Board refreshes once now, then the timer refreshes it every 3 seconds.
            this.Shown += async (s, e) =>
            {
                menuGrid.ClearSelection();
                menuGrid.CurrentCell = null;

                await RefreshBoardAsync();
                refreshTimer.Start();
            };

            // Stop the timer when the window closes.
            this.FormClosing += (s, e) => refreshTimer.Stop();

            refreshTimer.Interval = 3000;
            refreshTimer.Tick += async (s, e) => await RefreshBoardAsync();

            try
            {
                ReloadGrid();
            }
            catch (IOException)
            {
                MessageBox.Show("Couldn't load the menu. Please try again.", "MenuMate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BuildHeader()
        {
            Panel header = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = Green };

            Label title = new Label
            {
                Text = "MenuMate",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 8)
            };

            Label subtitle = new Label
            {
                Text = "Owner dashboard",
                ForeColor = Color.FromArgb(200, 225, 210),
                Font = new Font("Segoe UI", 9f),
                AutoSize = true,
                Location = new Point(22, title.Top + title.PreferredHeight - 2)
            };
            header.Height = subtitle.Top + subtitle.PreferredHeight + 12;

            Label signedIn = new Label
            {
                Text = "Signed in as " + currentUser.Username,
                ForeColor = Color.FromArgb(220, 235, 225),
                Font = new Font("Segoe UI", 9.5f),
                AutoSize = true
            };

            Button logoutButton = new Button
            {
                Text = "Log out",
                Size = new Size(100, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Green,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            logoutButton.FlatAppearance.BorderColor = Color.White;
            logoutButton.FlatAppearance.BorderSize = 1;
            logoutButton.FlatAppearance.MouseOverBackColor = GreenHover;
            logoutButton.FlatAppearance.MouseDownBackColor = GreenHover;
            logoutButton.Click += (s, e) => this.Close();

            // Keep Log out on the right edge and "Signed in as" just to its left.
            void PlaceRightSide()
            {
                logoutButton.Location = new Point(
                    header.ClientSize.Width - logoutButton.Width - 20,
                    (header.Height - logoutButton.Height) / 2);
                signedIn.Location = new Point(
                    logoutButton.Left - signedIn.PreferredWidth - 16,
                    (header.Height - signedIn.PreferredHeight) / 2);
            }
            header.Resize += (s, e) => PlaceRightSide();

            header.Controls.Add(title);
            header.Controls.Add(subtitle);
            header.Controls.Add(signedIn);
            header.Controls.Add(logoutButton);
            this.Controls.Add(header);
            PlaceRightSide();
        }

        private void BuildTabs()
        {
            tabs.Dock = DockStyle.Fill;
            tabs.DrawMode = TabDrawMode.OwnerDrawFixed;   // lets us paint the tabs ourselves
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.ItemSize = new Size(160, 40);
            tabs.DrawItem += Tabs_DrawItem;

            TabPage menuPage = NewPage("Menu Items");
            TabPage kitchenPage = NewPage("Kitchen Board");
            TabPage salesPage = NewPage("Sales");

            // Placeholder until task A4
            salesPage.Controls.Add(PlaceholderLabel());

            BuildMenuItemsTab(menuPage);
            BuildKitchenTab(kitchenPage);

            tabs.TabPages.Add(menuPage);
            tabs.TabPages.Add(kitchenPage);
            tabs.TabPages.Add(salesPage);
            this.Controls.Add(tabs);
        }

        // Paints each tab: bold green text with an orange underline if selected, plain gray text if not.
        private void Tabs_DrawItem(object? sender, DrawItemEventArgs e)
        {
            bool selected = e.Index == tabs.SelectedIndex;

            using (SolidBrush back = new SolidBrush(Cream))
            {
                e.Graphics.FillRectangle(back, e.Bounds);
            }

            using (Font tabFont = new Font(tabs.Font, selected ? FontStyle.Bold : FontStyle.Regular))
            {
                TextRenderer.DrawText(e.Graphics, tabs.TabPages[e.Index].Text, tabFont, e.Bounds,
                    selected ? Green : Muted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            if (selected)
            {
                using (SolidBrush underline = new SolidBrush(Orange))
                {
                    e.Graphics.FillRectangle(underline, e.Bounds.Left + 24, e.Bounds.Bottom - 4, e.Bounds.Width - 48, 3);
                }
            }
        }

        // =====================================================================
        // Kitchen Board tab (A3)
        // =====================================================================
        private void BuildKitchenTab(TabPage page)
        {
            // Three equal columns
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            layout.Controls.Add(BuildStageColumn(receivedList, receivedTitle, "Received (0)",
                "Start preparing", "Received", new Padding(0, 0, 12, 0)), 0, 0);
            layout.Controls.Add(BuildStageColumn(preparingList, preparingTitle, "Preparing (0)",
                "Mark as Ready", "Preparing", new Padding(0, 0, 12, 0)), 1, 0);
            layout.Controls.Add(BuildStageColumn(readyList, readyTitle, "Ready (0)",
                "Mark as Picked up", "Ready", new Padding(0)), 2, 0);

            page.Controls.Add(layout);
        }

        // One white card: title on top, the list in the middle, the orange button at the bottom.
        private Panel BuildStageColumn(ListBox list, Label title, string titleText,
            string buttonText, string stage, Padding margin)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(16),
                Margin = margin
            };

            title.Text = titleText;
            title.Dock = DockStyle.Top;
            title.Height = 38;
            title.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            title.ForeColor = DarkText;
            title.TextAlign = ContentAlignment.MiddleLeft;

            list.Dock = DockStyle.Fill;
            list.BorderStyle = BorderStyle.None;
            list.BackColor = Color.White;
            list.DrawMode = DrawMode.OwnerDrawFixed;
            list.ItemHeight = 64;
            list.IntegralHeight = false;
            list.DrawItem += OrderList_DrawItem;

            Button stageButton = MakeButton(buttonText, 0, 0, Orange, Color.White, Color.Empty, OrangeDark, true);
            stageButton.Dock = DockStyle.Fill;
            stageButton.Click += (s, e) => MoveSelectedOrder(list, stage);

            // The button sits in its own strip with a little space above it.
            Panel buttonStrip = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                Padding = new Padding(0, 12, 0, 0)
            };
            buttonStrip.Controls.Add(stageButton);

            // Fill first, then Bottom, then Top
            card.Controls.Add(list);
            card.Controls.Add(buttonStrip);
            card.Controls.Add(title);
            return card;
        }

        // Draws one order: soft green when selected, striped otherwise, text wraps over two or three lines.
        private void OrderList_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not ListBox list || e.Index < 0)
            {
                return;
            }

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color back = selected ? SelectGreen : (e.Index % 2 == 0 ? Color.White : RowAlt);

            using (SolidBrush brush = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            Rectangle textArea = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 4, e.Bounds.Width - 20, e.Bounds.Height - 8);
            string text = list.Items[e.Index].ToString() ?? "";
            TextRenderer.DrawText(e.Graphics, text, list.Font, textArea, DarkText,
                TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
        }

        // The orange button was clicked: move the selected order one stage forward.
        private async void MoveSelectedOrder(ListBox list, string stage)
        {
            if (list.SelectedItem is not OrderRow row)
            {
                ShowWarning("Select an order first.");
                return;
            }

            try
            {
                // Always work on the freshest data from the file, not on what is on screen.
                List<Order> orders = dataManager.LoadOrders();
                Order? order = orders.FirstOrDefault(o => o.OrderNumber == row.Order.OrderNumber);

                // Only move it if nobody has moved it already (for example, a second copy of the app).
                if (order != null && order.Status == stage)
                {
                    order.MoveToNextStage();
                    dataManager.SaveOrders(orders);
                }

                await RefreshBoardAsync();
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                ShowWarning("Couldn't update the order. Please try again.");
            }
        }

        // Reloads the orders in the background and rebuilds the three lists.
        private async Task RefreshBoardAsync()
        {
            // Never run two refreshes at the same time.
            if (isRefreshing)
            {
                return;
            }
            isRefreshing = true;

            try
            {
                List<Order>? orders = await dataManager.LoadOrdersAsync();

                // The window may have been closed while we were waiting.
                if (this.IsDisposed)
                {
                    return;
                }

                // null = the file was busy, so skip this refresh and try again at the next tick.
                if (orders == null)
                {
                    return;
                }

                kitchen = new Kitchen { Orders = orders };

                FillList(receivedList, receivedTitle, "Received");
                FillList(preparingList, preparingTitle, "Preparing");
                FillList(readyList, readyTitle, "Ready");
            }
            finally
            {
                isRefreshing = false;
            }
        }

        // Fills one list with the orders at a stage and keeps the same order selected.
        private void FillList(ListBox list, Label title, string stage)
        {
            int? selectedNumber = (list.SelectedItem as OrderRow)?.Order.OrderNumber;
            int oldTopIndex = list.TopIndex;

            List<Order> orders = kitchen.GetOrdersAt(stage);

            list.BeginUpdate();
            list.Items.Clear();
            foreach (Order order in orders)
            {
                int position = list.Items.Add(new OrderRow(order));
                if (selectedNumber != null && order.OrderNumber == selectedNumber)
                {
                    list.SelectedIndex = position;
                }
            }
            if (list.Items.Count > 0)
            {
                list.TopIndex = Math.Min(oldTopIndex, list.Items.Count - 1);
            }
            list.EndUpdate();

            title.Text = stage + " (" + orders.Count + ")";
        }

        // =====================================================================
        // Menu Items tab (A2)
        // =====================================================================
        private void BuildMenuItemsTab(TabPage page)
        {
            // Two columns: the table card on the left (fills), the details card on the right (340 px).
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340f));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // ----- Left card: title row + table -----
            Panel tableCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(16),
                Margin = new Padding(0, 0, 12, 0)
            };

            ConfigureGrid();

            Panel titleRow = new Panel { Dock = DockStyle.Top, Height = 40 };
            Label tableTitle = new Label
            {
                Text = "Menu",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = DarkText,
                TextAlign = ContentAlignment.MiddleLeft
            };
            countLabel.Dock = DockStyle.Right;
            countLabel.Width = 140;
            countLabel.TextAlign = ContentAlignment.MiddleRight;
            countLabel.ForeColor = Muted;
            countLabel.Font = new Font("Segoe UI", 9.5f);
            titleRow.Controls.Add(tableTitle);
            titleRow.Controls.Add(countLabel);

            // Fill first, then Top
            tableCard.Controls.Add(menuGrid);
            tableCard.Controls.Add(titleRow);

            // ----- Right card: details form -----
            Panel details = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            Label detailsTitle = new Label
            {
                Text = "Item details",
                AutoSize = true,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Location = new Point(20, 16)
            };

            Label hint = new Label
            {
                Text = "Pick an item in the table to edit it, or fill in the fields to add a new one.",
                AutoSize = true,
                MaximumSize = new Size(300, 0),
                Font = new Font("Segoe UI", 9f),
                ForeColor = Muted,
                Location = new Point(20, 48)
            };

            nameBox.Location = new Point(20, 118);
            nameBox.Width = 300;

            categoryBox.Location = new Point(20, 182);
            categoryBox.Width = 300;
            categoryBox.DropDownStyle = ComboBoxStyle.DropDownList;
            categoryBox.Items.AddRange(new object[] { "Drinks", "Food", "Desserts" });
            categoryBox.SelectedIndex = 0;

            priceBox.Location = new Point(20, 246);
            priceBox.Width = 140;
            priceBox.DecimalPlaces = 2;
            priceBox.Minimum = 0.25m;
            priceBox.Maximum = 100m;
            priceBox.Increment = 0.25m;
            priceBox.Value = 0.25m;

            prepBox.Location = new Point(180, 246);
            prepBox.Width = 140;
            prepBox.Minimum = 1;
            prepBox.Maximum = 60;
            prepBox.Value = 1;

            Button addButton = MakeButton("Add item", 20, 308, Orange, Color.White, Color.Empty, OrangeDark, true);
            Button updateButton = MakeButton("Update selected", 20, 356, Color.White, Green, Green, GreenSoft, false);
            Button removeButton = MakeButton("Remove selected", 20, 404, Color.White, Red, Red, RedSoft, false);
            Button clearButton = MakeButton("Clear", 20, 456, Color.White, Muted, Color.Empty, Cream, false);
            addButton.Click += AddButton_Click;
            updateButton.Click += UpdateButton_Click;
            removeButton.Click += RemoveButton_Click;
            clearButton.Click += ClearButton_Click;

            details.Controls.Add(detailsTitle);
            details.Controls.Add(hint);
            details.Controls.Add(FieldLabel("Name", 20, 96));
            details.Controls.Add(nameBox);
            details.Controls.Add(FieldLabel("Category", 20, 160));
            details.Controls.Add(categoryBox);
            details.Controls.Add(FieldLabel("Price ($)", 20, 224));
            details.Controls.Add(priceBox);
            details.Controls.Add(FieldLabel("Prep minutes", 180, 224));
            details.Controls.Add(prepBox);
            details.Controls.Add(addButton);
            details.Controls.Add(updateButton);
            details.Controls.Add(removeButton);
            details.Controls.Add(clearButton);

            layout.Controls.Add(tableCard, 0, 0);
            layout.Controls.Add(details, 1, 0);
            page.Controls.Add(layout);
        }

        // All the table styling in one place.
        private void ConfigureGrid()
        {
            menuGrid.Dock = DockStyle.Fill;
            menuGrid.ReadOnly = true;
            menuGrid.AllowUserToAddRows = false;
            menuGrid.AllowUserToDeleteRows = false;
            menuGrid.AllowUserToResizeRows = false;
            menuGrid.AllowUserToResizeColumns = false;
            menuGrid.MultiSelect = false;
            menuGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            menuGrid.RowHeadersVisible = false;
            menuGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            menuGrid.BackgroundColor = Color.White;
            menuGrid.BorderStyle = BorderStyle.None;
            menuGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            menuGrid.GridColor = Line;
            menuGrid.RowTemplate.Height = 34;

            // Header row
            menuGrid.EnableHeadersVisualStyles = false;
            menuGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            menuGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            menuGrid.ColumnHeadersHeight = 40;
            menuGrid.ColumnHeadersDefaultCellStyle.BackColor = Green;
            menuGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            menuGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Green;
            menuGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            menuGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            menuGrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            menuGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Body rows
            menuGrid.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            menuGrid.DefaultCellStyle.SelectionBackColor = SelectGreen;
            menuGrid.DefaultCellStyle.SelectionForeColor = DarkText;
            menuGrid.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            menuGrid.AlternatingRowsDefaultCellStyle.SelectionBackColor = SelectGreen;
            menuGrid.AlternatingRowsDefaultCellStyle.SelectionForeColor = DarkText;

            menuGrid.Columns.Add("Name", "Name");
            menuGrid.Columns.Add("Category", "Category");
            menuGrid.Columns.Add("Price", "Price");
            menuGrid.Columns.Add("Prep", "Prep (min)");

            menuGrid.Columns["Name"]!.FillWeight = 36;
            menuGrid.Columns["Category"]!.FillWeight = 24;
            menuGrid.Columns["Price"]!.FillWeight = 16;
            menuGrid.Columns["Prep"]!.FillWeight = 24;

            // Numbers line up on the right
            foreach (string columnName in new[] { "Price", "Prep" })
            {
                menuGrid.Columns[columnName]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                menuGrid.Columns[columnName]!.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            menuGrid.CellClick += MenuGrid_CellClick;
        }

        // Loads the menu from the file and shows it in the table. May throw IOException.
        private void ReloadGrid()
        {
            shownMenu = dataManager.LoadMenu();

            menuGrid.Rows.Clear();
            foreach (MenuItem item in shownMenu)
            {
                menuGrid.Rows.Add(
                    item.Name,
                    item.Category,
                    "$" + item.Price.ToString("0.00", CultureInfo.InvariantCulture),
                    item.PrepMinutes);
            }

            countLabel.Text = shownMenu.Count + (shownMenu.Count == 1 ? " item" : " items");

            menuGrid.ClearSelection();
            menuGrid.CurrentCell = null;
            ClearDetails();
        }

        // Empties the detail fields and forgets the selection.
        private void ClearDetails()
        {
            selectedName = null;
            nameBox.Text = "";
            categoryBox.SelectedIndex = 0;
            priceBox.Value = priceBox.Minimum;
            prepBox.Value = prepBox.Minimum;
        }

        // Clicking a row fills the detail fields.
        private void MenuGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= shownMenu.Count)
            {
                return;
            }

            MenuItem item = shownMenu[e.RowIndex];
            selectedName = item.Name;
            nameBox.Text = item.Name;
            categoryBox.SelectedItem = item.Category;
            priceBox.Value = Math.Clamp(item.Price, priceBox.Minimum, priceBox.Maximum);
            prepBox.Value = Math.Clamp((decimal)item.PrepMinutes, prepBox.Minimum, prepBox.Maximum);
        }

        // ----- Clear -----
        private void ClearButton_Click(object? sender, EventArgs e)
        {
            menuGrid.ClearSelection();
            menuGrid.CurrentCell = null;
            ClearDetails();
        }

        // ----- Add -----
        private void AddButton_Click(object? sender, EventArgs e)
        {
            string name = nameBox.Text.Trim();
            string? problem = CheckName(name);
            if (problem != null)
            {
                ShowWarning(problem);
                return;
            }

            try
            {
                List<MenuItem> menu = dataManager.LoadMenu();

                if (NameExists(menu, name, null))
                {
                    ShowWarning("An item called \"" + name + "\" is already on the menu.");
                    return;
                }

                menu.Add(new MenuItem
                {
                    Name = name,
                    Category = categoryBox.SelectedItem?.ToString() ?? "Drinks",
                    Price = Math.Round(priceBox.Value, 2),
                    PrepMinutes = (int)prepBox.Value
                });

                dataManager.SaveMenu(menu);
                ReloadGrid();
            }
            catch (IOException)
            {
                ShowWarning("Couldn't save the menu. Please try again.");
            }
        }

        // ----- Update -----
        private void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedName == null)
            {
                ShowWarning("Click an item in the table first.");
                return;
            }

            string oldName = selectedName;
            string name = nameBox.Text.Trim();
            string? problem = CheckName(name);
            if (problem != null)
            {
                ShowWarning(problem);
                return;
            }

            try
            {
                List<MenuItem> menu = dataManager.LoadMenu();

                MenuItem? existing = menu.FirstOrDefault(m =>
                    string.Equals(m.Name, oldName, StringComparison.OrdinalIgnoreCase));

                if (existing == null)
                {
                    ShowWarning("That item is no longer on the menu. The table has been refreshed.");
                    ReloadGrid();
                    return;
                }

                // If it was renamed, the new name must not belong to another item.
                if (NameExists(menu, name, oldName))
                {
                    ShowWarning("An item called \"" + name + "\" is already on the menu.");
                    return;
                }

                existing.Name = name;
                existing.Category = categoryBox.SelectedItem?.ToString() ?? "Drinks";
                existing.Price = Math.Round(priceBox.Value, 2);
                existing.PrepMinutes = (int)prepBox.Value;

                dataManager.SaveMenu(menu);
                ReloadGrid();
            }
            catch (IOException)
            {
                ShowWarning("Couldn't save the menu. Please try again.");
            }
        }

        // ----- Remove -----
        private void RemoveButton_Click(object? sender, EventArgs e)
        {
            if (selectedName == null)
            {
                ShowWarning("Click an item in the table first.");
                return;
            }

            string nameToRemove = selectedName;

            DialogResult answer = MessageBox.Show(
                "Remove " + nameToRemove + " from the menu?", "MenuMate",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            try
            {
                List<MenuItem> menu = dataManager.LoadMenu();
                menu.RemoveAll(m => string.Equals(m.Name, nameToRemove, StringComparison.OrdinalIgnoreCase));
                dataManager.SaveMenu(menu);
                ReloadGrid();
            }
            catch (IOException)
            {
                ShowWarning("Couldn't save the menu. Please try again.");
            }
        }

        // ===== Small helpers =====

        // Returns a message if the name is not allowed, or null if it is fine.
        private string? CheckName(string name)
        {
            if (name.Length == 0)
            {
                return "Please enter a name for the item.";
            }
            if (name.Contains('|') || name.Contains(':') || name.Contains(';'))
            {
                return "The name can't contain the characters | : or ;";
            }
            return null;
        }

        // True if another item already has this name (upper/lower case ignored).
        // ignoreName is the item's own current name, so it doesn't count as a duplicate of itself.
        private bool NameExists(List<MenuItem> menu, string name, string? ignoreName)
        {
            return menu.Any(m =>
                string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase) &&
                (ignoreName == null || !string.Equals(m.Name, ignoreName, StringComparison.OrdinalIgnoreCase)));
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "MenuMate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private TabPage NewPage(string text)
        {
            return new TabPage(text)
            {
                BackColor = Cream,
                UseVisualStyleBackColor = false,
                Padding = new Padding(16)
            };
        }

        private Label PlaceholderLabel()
        {
            return new Label
            {
                Text = "Coming soon",
                AutoSize = true,
                ForeColor = Muted,
                Font = new Font("Segoe UI", 12f),
                Location = new Point(24, 24)
            };
        }

        // Small gray label above an input field
        private Label FieldLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Muted,
                Location = new Point(x, y)
            };
        }

        // One helper for all the buttons: filled, outlined or plain, with a hover color.
        // Pass Color.Empty as border for no border.
        private Button MakeButton(string text, int x, int y, Color back, Color fore, Color border, Color hover, bool bold)
        {
            Button b = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(300, 40),
                BackColor = back,
                ForeColor = fore,
                Font = new Font("Segoe UI", 10f, bold ? FontStyle.Bold : FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            if (border.IsEmpty)
            {
                b.FlatAppearance.BorderSize = 0;
            }
            else
            {
                b.FlatAppearance.BorderColor = border;
                b.FlatAppearance.BorderSize = 1;
            }

            b.FlatAppearance.MouseOverBackColor = hover;
            b.FlatAppearance.MouseDownBackColor = hover;
            return b;
        }
    }
}