using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace htyö_GUI.Classes
{
    //This class initializes the Database. 
    //It has 2 separate connection strings, one is used if the DB doesn't exist, and the other if DB exists.
    public class DbInitializer
    {
        private const string localWithoutDb = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1;";
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1; Database=invoicedb;";
        public void InitializeDatabase()
        {
            string createDatabaseQuery = @"
                DROP DATABASE IF EXISTS `invoicedb`;
                CREATE DATABASE IF NOT EXISTS `invoicedb` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci */;
            ";

            string createTablesAndInsertsQuery = @"
                USE `invoicedb`;

                -- Create Customer table
                CREATE TABLE IF NOT EXISTS Customer (
                  customer_id INT PRIMARY KEY AUTO_INCREMENT,
                  customer_name VARCHAR(50) NOT NULL
                );

                -- Create Product table
                CREATE TABLE IF NOT EXISTS Product (
                  product_id INT PRIMARY KEY AUTO_INCREMENT,
                  product_name VARCHAR(255) NOT NULL,
                  billingType VARCHAR(50),
                  unitprice DECIMAL(10,2) NOT NULL
                );

                -- Create Invoice table
                CREATE TABLE IF NOT EXISTS Invoice (
                  invoice_id INT PRIMARY KEY AUTO_INCREMENT,
                  customer_id INT NOT NULL,
                  issue_date DATE,
                  expiration_date DATE,
                  zipcode_customer VARCHAR(20),
                  country_customer VARCHAR(100) NOT NULL,
                  city_customer VARCHAR(50) NOT NULL,
                  address_billing VARCHAR(100) NOT NULL,
                  city_biller VARCHAR(50) NOT NULL,
                  address_biller VARCHAR(100) NOT NULL,
                  zipcode_biller VARCHAR(20) NOT NULL,
                  FOREIGN KEY (customer_id) REFERENCES Customer(customer_id)
                );

                CREATE TABLE IF NOT EXISTS InvoiceItems (
                  invoiceitem_id INT PRIMARY KEY AUTO_INCREMENT,
                  invoice_id INT NOT NULL,
                  product_id INT NOT NULL,
                  amount INT NOT NULL,
                  unit_price DECIMAL(10,2) NOT NULL,
                  FOREIGN KEY (invoice_id) REFERENCES Invoice(invoice_id),
                  FOREIGN KEY (product_id) REFERENCES Product(product_id)
                );

                -- INSERTS

                -- Customers
                INSERT INTO Customer (customer_name) VALUES ('Matti');
                INSERT INTO Customer (customer_name) VALUES ('Jaakko');

                -- Products
                INSERT INTO Product (product_name, billingType, unitprice) VALUES ('Työ', 'Per hour', 60);
                INSERT INTO Product (product_name, billingType, unitprice) VALUES ('Silikoni', 'Per unit', 5.95);
                INSERT INTO Product (product_name, billingType, unitprice) VALUES ('Tiiviste', 'Per unit', 1);
                INSERT INTO Product (product_name, billingType, unitprice) VALUES ('Kattopaneeli', 'Per unit', 1.95);
                INSERT INTO Product (product_name, billingType, unitprice) VALUES ('Tiili', 'Per unit', 2);

                -- Invoices
                INSERT INTO Invoice (customer_id, issue_date, expiration_date, address_biller, address_billing, country_customer, city_customer, zipcode_customer, city_biller, zipcode_biller)
                  VALUES (1, CURDATE(), DATE_ADD(CURDATE(), INTERVAL 30 DAY), 'Business Address 1', 'Billing Address 1', 'FI Suomi', 'Joensuu', '80100', 'Joensuu', '80100');

                INSERT INTO InvoiceItems(invoice_id, product_id, amount, unit_price) VALUES (1, 1, 5, 60.00);
                INSERT INTO InvoiceItems(invoice_id, product_id, amount, unit_price) VALUES (1, 2, 1, 5.95);
                INSERT INTO InvoiceItems(invoice_id, product_id, amount, unit_price) VALUES (1, 3, 2, 1);
                INSERT INTO InvoiceItems(invoice_id, product_id, amount, unit_price) VALUES (1, 4, 80, 1.95);

                INSERT INTO Invoice (customer_id, issue_date, expiration_date, address_biller, address_billing, country_customer, city_customer, zipcode_customer, city_biller, zipcode_biller)
                  VALUES (2, CURDATE(), DATE_ADD(CURDATE(), INTERVAL 30 DAY), 'Business Address 1', 'Rakennuskatu 1', 'FI Suomi', 'Helsinki', '00280', 'Joensuu', '80100');

                INSERT INTO InvoiceItems(invoice_id, product_id, amount, unit_price) VALUES (2, 1, 5, 60.00);
                INSERT INTO InvoiceItems(invoice_id, product_id, amount, unit_price) VALUES (2, 2, 1, 5.95);
                INSERT INTO InvoiceItems(invoice_id, product_id, amount, unit_price) VALUES (2, 3, 2, 1);
                INSERT INTO InvoiceItems(invoice_id, product_id, amount, unit_price) VALUES (2, 4, 80, 1.95);
            ";

            using (MySqlConnection connection = new MySqlConnection(localWithoutDb))
            {
                MySqlCommand command = new MySqlCommand(createDatabaseQuery, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return;
                }
            }

            using (MySqlConnection connection = new MySqlConnection(localWithDb))
            {
                MySqlCommand command = new MySqlCommand(createTablesAndInsertsQuery, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Database and tables created successfully.")
                    ;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

    }
}
