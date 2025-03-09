using PustokApp.Data;
using PustokApp.Models.Home;

namespace PustokApp.Services
{
    public class LayoutService(PustokAppContext context)
    {
        public Dictionary<string,string> GetSettings()
        {
            return context.Setting
                .ToDictionary(s => s.Key, s => s.Value);
        }
        public List<Brand> GetBrands()
        {
            return context.Brand.ToList();
        }
    }
}
