using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymProgUI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class ProgamDrillContentView : ContentView
	{
        private bool _isEditable;
        
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                _isEditable = value;
            }
        }
        public ProgamDrillContentView ()
		{
			InitializeComponent ();
		}
	}
}