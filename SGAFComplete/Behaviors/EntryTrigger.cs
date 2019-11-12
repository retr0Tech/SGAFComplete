using System;
using System.Linq;
using SGAFComplete.ViewModels;
using Xamarin.Forms;

namespace SGAFComplete.Behaviors
{
    
    public class EntryTrigger : TriggerAction<Entry>
    {
        protected async override void Invoke(Entry entry)
        {
            try
            {
                var dataEmpty = MainCapturaDeActivosViewModel.data.Where(i => i.ToLower().Contains(entry.Text.ToLower()));
                MainCapturaDeActivosViewModel.decrpData.Clear();
                foreach (var element in dataEmpty)
                {
                    MainCapturaDeActivosViewModel.decrpData.Add(element);
                }

            }
            catch (Exception ex)
            {



            }
        }
    }
    
}
