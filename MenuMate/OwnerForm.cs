namespace MenuMate
{
    public partial class OwnerForm : Form
    {
        private readonly User currentUser;
        private readonly DataManager dataManager = new DataManager();

        public OwnerForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            this.Icon = Icon.FromHandle(Properties.Resources.logo.GetHicon());
            // TODO (Abdulquadri): BuildUi();
        }
    }
}