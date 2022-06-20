using OrderAssignmentDll.AllTableModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderAssignmentDll
{
    public class CRUDOperationForCustomers
    {
        DbContextFile dbContextFile;
        public CRUDOperationForCustomers()
        {
            dbContextFile = new DbContextFile();
        }
        private bool CustomerExistOrNot(string mail)
        {
            var customer = dbContextFile.customers.Where(cus => cus.Email == mail).FirstOrDefault();
            if (customer != null)
            {
                return true;
            }
            return false;
        }

        public void AddCustomers()
        {
        addmore:
            Customers customer = new Customers();
            try
            {
                
                try
                {
                    Console.WriteLine("Enter the First Name");
                    customer.FirstName = Console.ReadLine();
                    Console.WriteLine("Enter The Last Name");
                    customer.LastName = Console.ReadLine();
                    Console.WriteLine("Enter the Phone Number It Should be Cantain Only 10 Digits Number no more no less");
                    customer.Phone_No = Convert.ToInt64(Console.ReadLine());
                EnterValidmail:
                    Console.WriteLine("Enter the Email Address");

                    customer.Email = Console.ReadLine();
                    if (!validmail(customer.Email))
                    {
                        Console.WriteLine("Enter Valid Email Address => " + "it is not valid " + customer.Email);
                        goto EnterValidmail;
                    }
                }

                catch (FormatException ex)
                {
                    Console.WriteLine();
                    goto addmore;
                }
                if (customer.FirstName != "" && customer.LastName != "" && customer.Phone_No.ToString().Length == 10 && customer.Email != "")
                {

                    if (!CustomerExistOrNot(customer.Email) && validmail(customer.Email))
                    {
                        dbContextFile.customers.Add(customer);
                        dbContextFile.SaveChanges();

                        Console.WriteLine("Wait a Moments...");
                        emailsend(customer.Email, customer.FirstName, customer.LastName);
                        Console.WriteLine("Customer Add Successfully... ");
                        Console.WriteLine();
                        Console.WriteLine("Do you want to Add More Customers \n             press :1");
                        int check = Convert.ToInt32(Console.ReadLine());
                        if (check == 1)
                        {
                            goto addmore;
                        }

                    }
                    else
                    {
                        Console.WriteLine("User Already Exist Use Another   ");
                        goto addmore;
                    }





                }
                else
                {
                    Console.WriteLine(" You Missed Some Credential constraints");
                    Console.WriteLine("try Again");
                    goto addmore;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public void DeleteCustomers()
        {
            string Email;
        deleteAgain:
            Console.WriteLine("Enter The Valid Eamil That Customer You Want to delete");
            Email = Console.ReadLine();
            if (validmail(Email))
            {
                if (CustomerExistOrNot(Email))
                {
                    var deletecust=dbContextFile.customers.Where(cust=>cust.Email == Email).FirstOrDefault(); 
                    dbContextFile.customers.Remove(deletecust);
                    dbContextFile.SaveChanges();

                    Console.WriteLine("Customer Delete Successfully..");
                    Console.WriteLine();
                    Console.WriteLine("Do You Wnat To Delete More \n              press :1");
                    int check = int.Parse(Console.ReadLine());
                    if (check == 1)
                    {
                        goto deleteAgain;
                    }

                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(" Email Does not exist in the record");
                    goto deleteAgain;

                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Enter the Valid Email Address THis Email Format Not Correct => " + Email);
                goto deleteAgain;

            }
        }
        public void UpdateCustomers()
        {
            string newEmail;
            string Email;
        updateAgain:
            Customers customerss = new Customers();
            try
            {
                Console.WriteLine("Enter the Email That User you Want to Update");
                Email = Console.ReadLine();
                if (!validmail(Email))
                {
                    Console.WriteLine("Please Enter The Valid Email Adress");
                    goto updateAgain;
                }
                else
                {
                    if (CustomerExistOrNot(Email))
                    {
                    updateAgaindata:
                        try
                        {
                            Console.WriteLine("Enter The First Name");
                            customerss.FirstName = Console.ReadLine();
                            Console.WriteLine("Enter the Last Nmae");
                            customerss.LastName = Console.ReadLine();
                            Console.WriteLine("Enter The Phone Number");
                            customerss.Phone_No = Convert.ToInt64(Console.ReadLine());
                        validmailAddress:
                            Console.WriteLine("Enter New Email");
                            newEmail = Console.ReadLine();
                            customerss.Email = newEmail;
                            if (!validmail(newEmail))
                            {
                                Console.WriteLine("Email Address Is Not Valid  Put Valid Email");
                                goto validmailAddress;

                            }

                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message + "\nSome Information format Incorrect try Again");
                            goto updateAgaindata;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            goto updateAgaindata;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Customer Does not Exist with This Email Address  " + Email);
                        Console.WriteLine("Enter Again");
                        goto updateAgain;
                    }
                    if (customerss.FirstName != "" && customerss.LastName != "" && customerss.Phone_No.ToString().Length == 10 && !CustomerExistOrNot(newEmail))
                    {
                        var updatecust =dbContextFile.customers.Where(cust=> cust.Email== Email).FirstOrDefault();

                        updatecust.Email = customerss.Email;
                        updatecust.FirstName = customerss.FirstName; 
                        updatecust.LastName = customerss.LastName;
                        updatecust.Phone_No = customerss.Phone_No;
                        dbContextFile.customers.Update(updatecust);
                        dbContextFile.SaveChanges();

                        Console.WriteLine(" customers Update Successfully");

                        Console.WriteLine();
                        Console.WriteLine(" do you Want To UpDate More Customers\n                           press :1");
                        int check = int.Parse(Console.ReadLine());
                        if (check == 1)
                        {
                            goto updateAgain;
                        }
                    }
                    else
                    {
                        Console.WriteLine("  You Enter Some Detail Incorrect...");
                        Console.WriteLine(" Put Correct ForMat....");
                        goto updateAgain;
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }
        public void ListallCustomer()
        {
            var dt = dbContextFile.customers.ToList();
            
            try
            {
               
                if (dt!=null)
                {
                   
                    Console.WriteLine();

                    //print data
                    foreach(var cust in dt)
                    {
                        Console.WriteLine("Customer First Name     :"+ cust.FirstName);
                        Console.WriteLine("Customer Last Name      :" + cust.LastName);
                        Console.WriteLine("Customer  Phone number  : " + cust.Phone_No);
                        Console.WriteLine("Customer  Email         :" + cust.Email);
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No Data Found...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        private void CustomerMemu()
        {
            Console.WriteLine();
            Console.WriteLine("*****************************************************************");
            Console.WriteLine("Add Customer      Press :1                                      *");
            Console.WriteLine("Delete Customer   Press :2                                      *");
            Console.WriteLine("update Customer   Press :3                                      *");
            Console.WriteLine("Show All Customer Press :4                                      *");
            Console.WriteLine("Exit              Press :5                                      *");
            Console.WriteLine("*****************************************************************");
            Console.WriteLine();
        }
        public void emailsend(string to, string firstname, string lastname)
        {
            string from = "sinhgsanjay790043@gmail.com";
            string password = "wgmjdfbehcczelcy";
            string subject = "Welcome  Dear Customre";
            string body = "<h1>Dear, " + firstname + " " + lastname + "</h1>\nThanks for registering with us";
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(from);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = body;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public bool validmail(string email)
        {
            Regex eml = new Regex(@"^[a-zA-Z]+[._]{0,1}[0-9a-zA-Z]+[@][a-zA-Z]+[.][a-zA-Z]{2,3}([.]+[a-zA-Z]{2,3}){0,1}");
            Match m = eml.Match(email);
            if (m.Success)
            {
                return true;

            }
            else
            {
                return false;
            }

        }
        public void portal()
        {
        portalAgain:
            CustomerMemu();
            int choise = Convert.ToInt32(Console.ReadLine());
            switch (choise)
            {
                case 1:
                    AddCustomers();
                    goto portalAgain;
                case 2:
                    DeleteCustomers();
                    goto portalAgain;
                case 3:
                    UpdateCustomers();
                    goto portalAgain;
                case 4:
                    ListallCustomer();
                    goto portalAgain;
                case 5:
                    break;


                default:
                    Console.WriteLine(" Wrong Choise");
                    goto portalAgain;
            }


        }
    }
}
