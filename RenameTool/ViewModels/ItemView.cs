using RenameTool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenameTool.ViewModels
{
    class ItemView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Item _Item{ get; set; }

        public ItemView()
        {
            
        }
    }
}
