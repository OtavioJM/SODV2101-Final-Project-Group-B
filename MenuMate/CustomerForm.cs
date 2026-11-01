namespace MenuMate
{
    public partial class CustomerForm : Form
    {
        private readonly User currentUser;
        private readonly DataManager dataManager = new DataManager();

        public CustomerForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            this.Icon = Icon.FromHandle(Properties.Resources.logo.GetHicon());
            // TODO (Keane): BuildUi();
        }
    }
}
