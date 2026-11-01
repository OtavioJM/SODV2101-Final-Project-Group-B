namespace MenuMate
{
    public partial class LoginForm : Form
    {
        private readonly DataManager dataManager = new DataManager();

        public LoginForm()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Properties.Resources.logo.GetHicon());
            // TODO (Otavio): BuildUi();
        }
    }
}
