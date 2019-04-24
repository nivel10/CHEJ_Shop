namespace CHEJ_Shop.Common.Models
{
    using Newtonsoft.Json;
    using System;

    public class Product
    {
        #region Properties

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("lastPurchase")]
        public DateTime? LastPurchase { get; set; }

        [JsonProperty("lastSale")]
        public DateTime? LastSale { get; set; }

        [JsonProperty("isAvailabe")]
        public bool IsAvailabe { get; set; }

        [JsonProperty("stock")]
        public long Stock { get; set; }

        #region Old Code

        //[JsonProperty("imageFullPath")]
        //public string ImageFullPath
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(this.ImageUrl.Trim()))
        //        {
        //            return $"http://chejconsultor.ddns.net:9002{this.ImageUrl.Trim().Substring(1)}";
        //        }

        //        return "http://chejconsultor.ddns.net:9002/images/NoImage.png";
        //    }
        //} 

        #endregion Old Code
        [JsonProperty("imageFullPath")]
        public string ImageFullPath { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        public byte[] ImageArray { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{this.Name} {this.Price:C2}";
        }

        #endregion Methods
    }
}