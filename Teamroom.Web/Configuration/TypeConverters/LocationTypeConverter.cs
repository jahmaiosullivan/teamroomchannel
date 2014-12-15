using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using HobbyClue.Web.ViewModels;
using Newtonsoft.Json;

namespace HobbyClue.Web.Configuration.TypeConverters
{
    public class LocationTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var locationJson = (string)value;
                if (!string.IsNullOrEmpty(locationJson))
                {
                    var googleLocation = JsonConvert.DeserializeObject<GoogleLocation>(locationJson);
                    var addressLongName = googleLocation.address_components.First(x => x.types.Contains("route")).long_name;
                    var city = googleLocation.address_components.First(x => x.types.Contains("political") && x.types.Contains("locality")).long_name;
                    var state = googleLocation.address_components.First(x => x.types.Contains("political") && x.types.Contains("administrative_area_level_1")).long_name;
                    var zipCode = Convert.ToInt32(googleLocation.address_components.First(x => x.types.Contains("postal_code")).long_name);

                    return new LocationViewModel
                    {
                        GoogleId = googleLocation.id,
                        Name = googleLocation.name,
                        Description = string.Empty,
                        GoogleGeometryJson = string.Format(@"{0},{1}", googleLocation.geometry.location.d, googleLocation.geometry.location.e),
                        PhoneNumber = googleLocation.formatted_phone_number,
                        Address = addressLongName,
                        City = city,
                        State = state,
                        ZipCode = zipCode
                    };
                }

            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(LocationViewModel) || base.CanConvertTo(context, destinationType); 
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var lvm = value as LocationViewModel;
            if(lvm != null )
            {
                 return JsonConvert.SerializeObject(new {
                                         lvm.GoogleId, 
                                         lvm.Address, 
                                         lvm.City, 
                                         lvm.Country, 
                                         lvm.Description, 
                                         lvm.GoogleGeometryJson, 
                                         lvm.Id, 
                                         lvm.Name, 
                                         lvm.PhoneNumber, 
                                         lvm.State, 
                                         lvm.ZipCode 
                                     });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        
    }
}