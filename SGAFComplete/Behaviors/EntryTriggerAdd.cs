using System;
using System.Linq;
using SGAFComplete.ViewModels;
using Xamarin.Forms;

namespace SGAFComplete.Behaviors
{
    public class EntryTriggerAdd : TriggerAction<Entry>
    {
        protected async override void Invoke(Entry entry)
        {
            try
            {
                var dataEmpty = AgregarDescripcionViewModel.data.Where(i => i.ToLower().Contains(entry.Text.ToLower()));
                AgregarDescripcionViewModel.decrpData.Clear();
                foreach (var element in dataEmpty)
                {
                    AgregarDescripcionViewModel.decrpData.Add(element);
                }

            }
            catch (Exception ex)
            {



            }
        }
    }
}
