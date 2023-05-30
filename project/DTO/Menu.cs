using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DTO
{
    internal class Menu
    {
        // Constuctor
        public Menu(string foodName, int quantity, float price, float totalPrice=0)
        {
            this.FoodName = foodName;
            this.Quantity = quantity;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public Menu(DataRow row)
        {
            this.FoodName = (string)row["fName"];
            this.Quantity = (int)row["quantity"];
            this.Price = (float)(double)row["price"];
            this.TotalPrice = (float)(double)row["totalPrice"];
        }

        // đưa vào những thông tin muốn show trong lsvView
        private string foodName;
        private int quantity;
        private float price;   
        private float totalPrice;

        public string FoodName { get => foodName; set => foodName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public float Price { get => price; set => price = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
