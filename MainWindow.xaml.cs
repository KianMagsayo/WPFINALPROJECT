using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MAGSAYOWPFINAL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        string[] names = new string[100];
        string[] addresses = new string[100];
        string[] payment = new string[100];
        int[] quantity = new int[100];
        double[] price = new double[100];
        double[] totalAmount = new double[100];
        string[] order = new string[100];
        int[] discount = new int[100];
        string[] statusOrder = new string[100];

        char status = 'A';
        int index = 0;
        private int updatedIndex = -1;



        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = txtCustomerName.Text;
            string address = txtAddress.Text; ;
            string order = comboboxOrder.Text;
            string payment = comboBoxPayment.Text;

            int qty;

            if (!int.TryParse(txtQuantity.Text, out qty))
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            string data = $"{name} - {address} - {payment} =  {order} = {qty} = {totalAmount} ";

            //pag gamit ramog TryParse para dle mag crash para ma convert ang string to integer since nag gamit mankog textbox dha sa price nga field

            if (!double.TryParse(txtPrice.Text, out double pr))
            {
                MessageBox.Show("Enter a valid price!", "Input Error", MessageBoxButton.OK);
                return;
            }
            double total = qty * pr;

            if (!double.TryParse(txtDiscount.Text, out double ds))
            {
                MessageBox.Show("Enter a valid discount!", "Input Error");
                return;
            }

            double discountAmount = total * (ds / 100);
            double final = total - discountAmount;
            if (ds >= 0 && ds <= 100)
            {
                MessageBox.Show($"This order received a {discountAmount} discount.", "DISCOUNT APPLLIED");

            }

            string data1 = $"{name}";
            if (name == "")
            {
                MessageBox.Show("Please add customer name", "Customer Details", MessageBoxButton.OK);
                return;
            }

            string data2 = $"{address}";
            if (address == "")
            {
                MessageBox.Show("Please provide customer address", "Customer Details", MessageBoxButton.OK);
                return;
            }

            string data3 = $"{payment}";
            if (payment == "")
            {
                MessageBox.Show("Please select payment", "Customer Details", MessageBoxButton.OK);
                return;
            }

            string data4 = $"{order}";
            if (order == "")
            {
                MessageBox.Show("Please add order", "Customer Details", MessageBoxButton.OK);
                return;
            }

            string data5 = $"{qty}";
            if (qty == 0)
            {
                MessageBox.Show("Please add quantity", "Customer Details", MessageBoxButton.OK);
                return;
            }

            SaveData(name, address, payment, order, qty, pr, ds, final);
            ClearData();
            OrderDeliver();
        }


        private void OrderDeliver()
        {
            txtCustomerName.Clear();
            txtAddress.Clear();
            comboBoxPayment.SelectedIndex = -1;
            comboboxOrder.SelectedIndex = -1;
            txtPrice.Clear();
            txtQuantity.Clear();
            //Quantity

        }

        private void ClearData()
        {
            txtCustomerName.Clear();
            txtAddress.Clear();
            comboBoxPayment.SelectedIndex = -1;
            comboboxOrder.SelectedIndex = -1;
            txtDiscount.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void SaveData(string n, string a, string p, string o, int q, double pr, double ds, double total)
        {
            if (status == 'A')
            {
                //save to array
                names[index] = n;
                addresses[index] = a;
                payment[index] = p;
                order[index] = o;
                quantity[index] = q;
                price[index] = pr;
                discount[index] = (int)ds;
                totalAmount[index] = total;
                statusOrder[index] = "Active";
                RefreshGrid();

                //add sa datagrid
                dataGrid.Items.Add(new
                {
                    CustomerName = names[index],
                    Address = addresses[index],
                    Payment = payment[index],
                    Order = order[index],
                    Quantity = quantity[index],
                    Price = price[index],
                    Discount = discount[index],
                    TotalAmount = totalAmount[index],
                    Status = statusOrder[index]

                });
                //increment index
                index++;

                MessageBox.Show("NEW ORDER SUCCESFULLY ADDED!", "CUSTOMER DETAIL", MessageBoxButton.OK);
                return;
            }
            else if (status == 'E' && updatedIndex >= 0)
            {
                //update data sa given index
                names[updatedIndex] = n;
                addresses[updatedIndex] = a;
                payment[updatedIndex] = p;
                order[updatedIndex] = o;
                quantity[updatedIndex] = q;
                price[updatedIndex] = pr;
                discount[updatedIndex] = (int)ds;
                totalAmount[updatedIndex] = total;

                //refresh datagrid
                RefreshGrid();

                //set sa default status ug updatedindex
                status = 'A';
                updatedIndex = -1;


                MessageBox.Show($"CUSTOMER ORDER SUCCESFULLY UPDATED!", "CUSTOMER DETAILS");
            }
        }


        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex >= 0)
            {
                txtCustomerName.Text = names[selectedIndex];
                txtAddress.Text = addresses[selectedIndex];
                comboboxOrder.Text = order[selectedIndex];
                comboBoxPayment.Text = payment[selectedIndex];
                txtDiscount.Text = discount[selectedIndex].ToString();
                txtQuantity.Text = quantity[selectedIndex].ToString();
                txtPrice.Text = price[selectedIndex].ToString();




                //para ma click ninyo ang button nga gi butangan ninyo didtos interface

                // since naka IsEnable=false man sila didtos xaml ninyo dre na ninyo e declared ang true
                btnDeleteData.IsEnabled = true;
                btnDeliver.IsEnabled = true;
                btnClose.IsEnabled = true;
                btnClearData.IsEnabled = true;

                status = 'E';
                updatedIndex = selectedIndex;
            }

        }


        private void btnDeleteData_Click_1(object sender, RoutedEventArgs e)
        {
            int deleteIndex = dataGrid.SelectedIndex;

            if (deleteIndex == -1)
            {
                MessageBox.Show("PLEASE SELECT A ROW TO DELETE");
                return;
            }

            string order = comboboxOrder.Text;
            ShiftElements(deleteIndex);
            //decrement
            index--;

            RefreshGrid();
            status = 'A';
            updatedIndex = -1;
            //para ma disable ang btnDeleteData_Click1
            btnDeleteData.IsEnabled = false;
            ClearData();
            status = 'A';
            updatedIndex = -1;
            //               mao niy content sa inyoang message |   | tas kani dre mao ni ang Heading or Title ba na
            MessageBox.Show($"Customer order ( {order} ) removed succesfully!", "CUSTOMER ORDER", MessageBoxButton.OK);
        }


        private void btnOrderDeliver_Click(object sender, RoutedEventArgs e)
        {
            int orderIndex = dataGrid.SelectedIndex;
            if (orderIndex == -1)
            {
                //pwede ninyo ilisan ang mga naa sa sulod anang MessageBox
                MessageBox.Show("SELECT A ROW TO DELIVER");
                return;
            }
            string name = txtCustomerName.Text;
            ShiftOrders(orderIndex);
            index--;
            Refresh();
            status = 'A';
            updatedIndex = -1;
            btnDeliver.IsEnabled = false;
            ClearData();
            MessageBox.Show($"{name} Order is being delivered!", "CUSTOMER ORDER", MessageBoxButton.OK);
        }


        private void btnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult firstCheck = MessageBox.Show("ARE  YOU SURE YOU WANT TO DELETE THIS RECORD?", "FIRST CONFIRMATION",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (firstCheck == MessageBoxResult.Yes)
            {
                MessageBoxResult secondCheck = MessageBox.Show("THIS ACTION WILL CLOSE THE MENU, DO YOU STILL WANT TO CONTINUE?", "SECOND CONFIRMATION", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (secondCheck == MessageBoxResult.Yes)
                {
                    MessageBox.Show("SUCCESFULLY CLOSED", "MENU", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    Application.Current.Shutdown();
                }

            }
        }

        private void btnClearData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show(
            "Are you sure you want to clear all existing records?",
            "CLEAR ALL",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                dataGrid.Items.Clear();

                names = new string[100];
                addresses = new string[100];
                payment = new string[100];
                order = new string[100];
                quantity = new int[100];
                price = new double[100];
                discount = new int[100];
                totalAmount = new double[100];
                index = 0;
                btnClearData.IsEnabled = true;
                btnDeleteData.IsEnabled = false;

                ClearData();
                MessageBox.Show("All customer records had been cleared!", "SUCCESS!");
            }
        }

        private async void btnPending_Click(object sender, RoutedEventArgs e)
        {
            int pendingIndex = dataGrid.SelectedIndex;


            if (pendingIndex == -1)
            {
                MessageBox.Show("SELECT A ROW TO SET AS PENDING");
                return;
            }

            string name = names[pendingIndex];

            statusOrder[pendingIndex] = "Pending";
            RefreshGrid();

            MessageBox.Show($"{name}'s order is now pending...", "PENDING");

            await Task.Delay(5000);
            if (pendingIndex < index)
            {
                ShiftOrders(pendingIndex);
                index--;
            }
            RefreshGrid();
            status = 'A';
            updatedIndex = -1;
            dataGrid.IsEnabled = true;
            ClearData();
            MessageBox.Show($"{name}'s order has been delivered.", "DONE");
        }

        private void Unsaved_Click (object sender, RoutedEventArgs e)
        {

            dataGrid.Items.Clear();

            names = new string[100];
            addresses = new string[100];
            payment = new string[100];
            order = new string[100];
            quantity = new int[100];
            price = new double[100];
            discount = new int[100];
            totalAmount = new double[100];
            index = 0;
            btnUnsave.IsEnabled = true;
            btnDeleteData.IsEnabled = false;

            ClearData();
        }

        // para ni ma clear ang details ig select nimo sa Deliver Order, same rani silag function sa btnDeleteData_Click
        private void Refresh()
        {
            dataGrid.Items.Clear();
            for (int i = 0; i < index; i++)
            {
                dataGrid.Items.Add(new
                {
                    CustomerName = names[i],
                    Address = addresses[i],
                    Payment = payment[i],
                    Order = order[i],
                    Quantity = quantity[i],
                    Price = price[i],
                    Discount = discount[i],
                    TotalAmount = totalAmount[i],
                    Status = statusOrder[i]
                });
            }
        }



        //kamo nay bahala kung unsa inyong method nga gamiton sa mga buttons na e dungang parihas ra ani

        //makita nimo ni didto sa btnOrderDeliver_Click Method

        //kumbaga mao niy mag add sa mga details paingon didto sa datagrid
        private void ShiftOrders(int orderIndex)
        {
            for (int i = orderIndex; i < index - 1; i++)
            {
                names[i] = names[i + 1];
                addresses[i] = addresses[i + 1];
                payment[i] = payment[i + 1];
                order[i] = order[i + 1];
                quantity[i] = quantity[i + 1];
                price[i] = price[i + 1];
                discount[i] = discount[i + 1];
                totalAmount[i] = totalAmount[i + 1];
                statusOrder[i] = statusOrder[i + 1];
            }
        }


        //para ni inig pili nimo didto sa datagrid ma remove ra
        private void RefreshGrid()
        {
            dataGrid.Items.Clear();
            for (int i = 0; i < index; i++)
            {
                dataGrid.Items.Add(new
                {
                    CustomerName = names[i],
                    Address = addresses[i],
                    Payment = payment[i],
                    Order = order[i],
                    Quantity = quantity[i],
                    Price = price[i],
                    Discount = discount[i],
                    TotalAmount = totalAmount[i],
                    Status = statusOrder[i]
                });
            }
        }


        //same  rani sa clear mao niy naka butang sa REMOVE SELECTED CUSTOMER ORDERS makita nimo ni didto sa btnDeleteData_Click Method

        private void ShiftElements(int deletedIndex)
        {
            for (int i = deletedIndex; i < index - 1; i++)
            {
                names[i] = names[i + 1];
                addresses[i] = addresses[i + 1];
                payment[i] = payment[i + 1];
                order[i] = order[i + 1];
                quantity[i] = quantity[i + 1];
                price[i] = price[i + 1];
                discount[i] = discount[i + 1];
                totalAmount[i] = totalAmount[i + 1];
                statusOrder[i] = statusOrder[i + 1];
            }
        }



    }
}