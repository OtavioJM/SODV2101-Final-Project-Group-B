namespace MenuMate
{
    public partial class LoginForm : Form
    {
        private readonly DataManager dataManager = new DataManager();

        // Brand colors from the brief
        private readonly Color green = ColorTranslator.FromHtml("#2F6B4F");
        private readonly Color orange = ColorTranslator.FromHtml("#E8833A");
        private readonly Color cream = ColorTranslator.FromHtml("#FAF6EF");
        private readonly Color darkText = ColorTranslator.FromHtml("#1F2A24");

        // Controls we need again later, inside the button clicks
        private readonly TextBox usernameBox = new TextBox();
        private readonly TextBox passwordBox = new TextBox();
        private readonly Label messageLabel = new Label();

        public LoginForm()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Properties.Resources.logo.GetHicon());
            BuildUi();
        }

        // Creates every control on the window in code.
        private void BuildUi()
        {
            // ----- The window -----
            this.Font = new Font("Segoe UI", 10);           // set the font first, before the size
            this.Text = "MenuMate - Log in";
            this.ClientSize = new Size(420, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = cream;

            // ----- Logo, title and slogan -----
            var logo = new PictureBox
            {
                Image = Properties.Resources.logo,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(140, 140),
                Location = new Point((420 - 140) / 2, 20)   // centered: (window width - logo width) / 2
            };

            var titleLabel = new Label
            {
                Text = "MenuMate",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = green,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 165),
                Size = new Size(420, 40)                    // full width, so the text is centered
            };

            var sloganLabel = new Label
            {
                Text = "Ordering made simple.",
                ForeColor = darkText,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 205),
                Size = new Size(420, 24)
            };

            // ----- Username and password -----
            var usernameLabel = new Label
            {
                Text = "Username",
                ForeColor = darkText,
                AutoSize = true,
                Location = new Point(60, 245)
            };
            usernameBox.Location = new Point(60, 268);
            usernameBox.Width = 300;

            var passwordLabel = new Label
            {
                Text = "Password",
                ForeColor = darkText,
                AutoSize = true,
                Location = new Point(60, 305)
            };
            passwordBox.Location = new Point(60, 328);
            passwordBox.Width = 300;
            passwordBox.UseSystemPasswordChar = true;       // shows dots instead of the password

            // ----- Buttons -----
            var loginButton = new Button
            {
                Text = "Log in",
                Location = new Point(60, 375),
                Size = new Size(300, 40),
                BackColor = orange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            loginButton.FlatAppearance.BorderSize = 0;      // main buttons have no border
            loginButton.Click += LoginButton_Click;
            this.AcceptButton = loginButton;                // pressing Enter clicks Log in

            var createButton = new Button
            {
                Text = "Create account",
                Location = new Point(60, 425),
                Size = new Size(300, 40),
                BackColor = Color.White,
                ForeColor = green,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            createButton.FlatAppearance.BorderColor = green;
            createButton.Click += CreateButton_Click;

            // ----- Message label (errors in red, success in green) -----
            messageLabel.Text = "";
            messageLabel.ForeColor = Color.Firebrick;
            messageLabel.TextAlign = ContentAlignment.TopCenter;
            messageLabel.Location = new Point(30, 480);
            messageLabel.Size = new Size(360, 50);

            // ----- Put everything on the window -----
            this.Controls.AddRange(new Control[]
            {
                logo, titleLabel, sloganLabel,
                usernameLabel, usernameBox,
                passwordLabel, passwordBox,
                loginButton, createButton, messageLabel
            });
        }
        // Shows a message under the buttons: red for errors, green for success.
        private void ShowMessage(string text, bool isError)
        {
            messageLabel.Text = text;
            messageLabel.ForeColor = isError ? Color.Firebrick : green;
        }
        // True if the text contains a character we use as a separator in the data files.
        private bool HasForbiddenCharacters(string text)
        {
            return text.Contains('|') || text.Contains(':') || text.Contains(';');
        }

        // Runs when "Log in" is clicked (or Enter is pressed).
        private void LoginButton_Click(object? sender, EventArgs e)
        {
            string username = usernameBox.Text.Trim();   // Trim removes spaces at the start and end
            string password = passwordBox.Text;

            // 1) Load the users from users.txt
            List<User> users;
            try
            {
                users = dataManager.LoadUsers();
            }
            catch (IOException)
            {
                ShowMessage("Couldn't read the accounts. Please try again.", true);
                return;
            }

            // 2) Find the user: username ignores upper/lower case, password must match exactly
            User? foundUser = null;
            foreach (User user in users)
            {
                if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase)
                    && user.Password == password)
                {
                    foundUser = user;
                    break;   // found it, stop looking
                }
            }

            if (foundUser == null)
            {
                ShowMessage("Wrong username or password.", true);
                return;
            }

            // 3) Open the right window for this user
            Form nextForm;
            if (foundUser.IsOwner())
            {
                nextForm = new OwnerForm(foundUser);
            }
            else
            {
                nextForm = new CustomerForm(foundUser);
            }

            // 4) When that window closes (Log out or X), come back to the login window
            nextForm.FormClosed += (s, args) =>
            {
                passwordBox.Clear();
                messageLabel.Text = "";
                this.Show();
                passwordBox.Focus();   // ready to type the next password
            };

            nextForm.Show();
            this.Hide();
        }

        // Runs when "Create account" is clicked. Always creates a Customer, never an Owner.
        private void CreateButton_Click(object? sender, EventArgs e)
        {
            string username = usernameBox.Text.Trim();
            string password = passwordBox.Text;

            // 1) Check the input before touching the file
            if (username == "" || password == "")
            {
                ShowMessage("Please enter a username and a password.", true);
                return;
            }

            if (HasForbiddenCharacters(username) || HasForbiddenCharacters(password))
            {
                ShowMessage("Username and password can't contain | : or ;", true);
                return;
            }

            try
            {
                // 2) Load the latest users right before saving (another window may have added one)
                List<User> users = dataManager.LoadUsers();

                // 3) The username must be new (ignore upper/lower case)
                foreach (User user in users)
                {
                    if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMessage("That username is already taken.", true);
                        return;
                    }
                }

                // 4) Add the new customer and save
                users.Add(new User
                {
                    Username = username,
                    Password = password,
                    Role = "Customer"   // always Customer; owners are set up ahead of time
                });
                dataManager.SaveUsers(users);
            }
            catch (IOException)
            {
                ShowMessage("Couldn't create the account. Please try again.", true);
                return;
            }

            ShowMessage("Account created. You can log in now.", false);
        }
    }
}