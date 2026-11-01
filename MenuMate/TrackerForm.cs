namespace MenuMate
{
    public partial class TrackerForm : Form
    {
        private readonly int orderNumber;
        private readonly DataManager dataManager = new DataManager();

        public TrackerForm(int orderNumber)
        {
            InitializeComponent();
            this.orderNumber = orderNumber;
            this.Icon = Icon.FromHandle(Properties.Resources.logo.GetHicon());
            // TODO (Keane): BuildUi();
        }
    }
}
