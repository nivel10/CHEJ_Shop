namespace CHEJ_Shop.UIClassic.Android.Adapters
{
    using Common.Models;
    using global::Android.App;
    using global::Android.Views;
    using global::Android.Widget;
    using Helpers;
    using System.Collections.Generic;

    public class ProductsListAdapter : BaseAdapter<Product>
    {
        #region Attributes

        private readonly List<Product> items;
        private readonly Activity context;

        #endregion Attributes

        #region Constructor

        public ProductsListAdapter(
          Activity _context,
          List<Product> _items) : base()
        {
            this.context = _context;
            this.items = _items;
        }

        #endregion Constructor

        #region Methods

        public override long GetItemId(
            int _position)
        {
            return _position;
        }

        public override Product this[int position] => items[position];

        public override int Count => items.Count;

        public override View GetView(
            int _position,
            View _convertView,
            ViewGroup _parent)
        {
            var item = items[_position];

            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(item.ImageFullPath);

            if (_convertView == null)
            {
                _convertView = context.LayoutInflater.Inflate(Resource.Layout.ProductRow, null);
            }

            _convertView.FindViewById<TextView>(Resource.Id.nameTextView).Text = item.Name;
            _convertView.FindViewById<TextView>(Resource.Id.priceTextView).Text = $"{item.Price:C2}";
            _convertView.FindViewById<ImageView>(Resource.Id.productImageView).SetImageBitmap(imageBitmap);

            return _convertView;
        }

        #endregion Methods
    }
}