namespace CHEJ_Shop.UIClaissic.iOS.DataSources
{
    using CHEJ_Shop.UIClaissic.iOS.Cells;
    using Common.Helpers;
    using Common.Models;
    using Foundation;
    using System;
    using System.Collections.Generic;
    using UIKit;

    public class ProductsDataSource : UITableViewSource
    {
        #region Attributes

        private readonly List<Product> products;
        private readonly NSString cellIdentifier = new NSString("ProductCell");

        #endregion Attributes

        #region Constructor

        public ProductsDataSource(
            List<Product> _products)
        {
            this.products = _products;
        }

        #endregion Constructor

        #region Methods

        #region Old Code

        //public override UITableViewCell GetCell(
        //    UITableView _tableView,
        //    NSIndexPath _indexPath)
        //{
        //    var cell = _tableView.DequeueReusableCell(cellIdentifier) as UITableViewCell;

        //    if (cell == null)
        //    {
        //        cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
        //    }

        //    #region Old Code

        //    //var product = products[_indexPath.Row];
        //    //cell.TextLabel.Text = product.Name;
        //    //cell.ImageView.Image = UIImage.FromFile(product.ImageFullPath);
        //    //return cell;

        //    #endregion Old Code

        //    var product = products[_indexPath.Row];
        //    cell.TextLabel.Text = product.Name;
        //    var imageData = this.GetImageData(product.ImageFullPath);
        //    cell.ImageView.Image = UIImage.LoadFromData(imageData);

        //    return cell;
        //} 

        #endregion Old Code

        public override UITableViewCell GetCell(
            UITableView _tableView,
            NSIndexPath _indexPath)
        {
            var cell = _tableView.DequeueReusableCell(cellIdentifier) as ProductCell;

            if (cell == null)
            {
                cell = new ProductCell(cellIdentifier);
            }

            #region Old Code

            //var product = products[_indexPath.Row];
            //cell.UpdateCell(
            //    product.Name, 
            //    $"{product.Price:C2}", 
            //    UIImage.FromFile(product.ImageUrl)); 

            #endregion Old Code

            var product = products[_indexPath.Row];
            var imageData = this.GetImageData(product.ImageFullPath);
            cell.UpdateCell(
                product.Name,
                $"{product.Price:C2}",
                UIImage.LoadFromData(imageData));

            return cell;
        }

        public override nint RowsInSection(
            UITableView _tableview,
            nint _section)
        {
            return this.products.Count;
        }

        private NSData GetImageData(
            string _imageFullPath)
        {
            var imageFullPath = new NSUrl(_imageFullPath);
            var imageData = NSData.FromUrl(imageFullPath);
            if (imageData == null)
            {
                imageData = NSData.FromUrl(new NSUrl(MethodsHelper.GetUrlNotImage));
            }

            return imageData;
        }

        #endregion Methods
    }
}