using MarketPlaceService.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceService.Utilities
{
    public class JsonUtility
    {
        public static Tuple<bool, JObject> GetDiffJson(JObject oldJson, JObject newJson, JsonType type)
        {
            var rules = ConfigureComparisonRules(type);
            return getDiffJson(oldJson, newJson, "/", rules);
        }

        public static Tuple<bool, JObject> getDiffJson(JObject oldJson, JObject newJson, string currentPath, Dictionary<string, Dictionary<string, string>> rules)
        {

            bool changeDetected = false;
            var resultJson = newJson.DeepClone();
            List<ChangeDetected> changedArray = new List<ChangeDetected> { };

            Dictionary<string, string> currentRule = null;
            Dictionary<string, string> ignoreList = new Dictionary<string, string>();
            if (rules.ContainsKey(currentPath))
            {
                currentRule = rules[currentPath];
                if (currentRule.ContainsKey("ignoreList"))
                    ignoreList = currentRule["ignoreList"].Split(',').ToDictionary(x => x, x => x);
            }

            var allProperties = oldJson.Properties().Select(o => o.Name).Union(newJson.Properties().Select(o => o.Name));
            foreach (var prop in allProperties)
            {
                if (ignoreList.ContainsKey(prop))
                    continue; //ignore this property because its in ignore list

                var subpath = "";
                if (!currentPath.EndsWith("/"))
                    subpath = currentPath + "/" + prop;
                else
                    subpath = currentPath + prop;

                if (!newJson.ContainsKey(prop))
                {
                    //property deleted
                    changedArray.Add(new ChangeDetected
                    {
                        ChangeType = "Deleted",
                        FieldName = prop,
                        Value = oldJson.Property(prop).Value.ToString()
                    });
                }
                else if (!oldJson.ContainsKey(prop))
                {//new object
                    changedArray.Add(new ChangeDetected
                    {
                        ChangeType = "Added",
                        FieldName = prop,
                        Value = newJson.Property(prop).Value.ToString()
                    });


                }
                else
                { //change property
                    if (oldJson.Property(prop).Value.ToString() != newJson.Property(prop).Value.ToString())
                    {
                        var oldVal = oldJson.Property(prop).Value;
                        var newVal = newJson.Property(prop).Value;
                        if (newVal.GetType() == typeof(JValue))
                        {
                            changedArray.Add(new ChangeDetected
                            {
                                ChangeType = "Changed",
                                FieldName = prop,
                                Value = oldJson.Property(prop).Value.ToString()
                            });
                        }
                        else if (newVal.GetType() == typeof(JObject))
                        {
                            JObject oldObject = oldJson.Property(prop).Value.Count() > 0 ? (JObject)oldJson.Property(prop).Value : new JObject();
                            var newobject = getDiffJson(oldObject, (JObject)newJson.Property(prop).Value, subpath, rules);
                            if (newobject.Item1)
                                changeDetected = true;
                            resultJson[prop] = newobject.Item2;
                        }
                        else if (newVal.GetType() == typeof(JArray))
                        {
                            //
                            Dictionary<string, string> currentArrayRule = null;
                            if (rules.ContainsKey(subpath))
                            {
                                currentArrayRule = rules[subpath];
                            }
                            var compareFieldName = currentArrayRule["idCol"];


                            var recordExists = newVal.Select(o => o[compareFieldName]).Intersect(oldVal.Select(o => o[compareFieldName]));
                            var insertRecord = newVal.Select(o => o[compareFieldName]).Except(oldVal.Select(o => o[compareFieldName]));
                            var deletedRecord = oldVal.Select(o => o[compareFieldName]).Except(newVal.Select(o => o[compareFieldName]));

                            if (deletedRecord.Count() > 0)
                            {
                                List<JObject> deletedchangedArrayList = new List<JObject>();
                                foreach (var objRecord in deletedRecord)
                                {
                                    deletedchangedArrayList.Add((JObject)oldJson.Property(prop).Value.First(o => o[compareFieldName] == objRecord));
                                }

                                changedArray.Add(new ChangeDetected
                                {
                                    ChangeType = "Deleted",
                                    FieldName = prop,
                                    Value = JToken.FromObject(deletedchangedArrayList).ToString()
                                });
                            }

                            if (insertRecord.Count() > 0)
                            {
                                List<JObject> insertedchangedArrayList = new List<JObject>();
                                foreach (var objRecord in insertRecord)
                                {
                                    insertedchangedArrayList.Add((JObject)newJson.Property(prop).Value.First(o => o[compareFieldName] == objRecord));
                                }

                                changedArray.Add(new ChangeDetected
                                {
                                    ChangeType = "Added",
                                    FieldName = prop,
                                    Value = JToken.FromObject(insertedchangedArrayList).ToString()
                                });
                            }

                            if (recordExists.Count() > 0)
                            {
                                foreach (var objRecord in recordExists)
                                {
                                    if (objRecord == null)
                                        continue;
                                    var oldObject = oldVal.First(o => o[compareFieldName].ToString().Equals(objRecord.ToString()));
                                    var newObject = newVal.First(o => o[compareFieldName].ToString().Equals(objRecord.ToString()));
                                    JArray newarray = (JArray)resultJson[prop];
                                    var newJsonObject = newarray.First(o => o[compareFieldName].ToString().Equals(objRecord.ToString()));

                                    var updatedObject = getDiffJson((JObject)oldObject, (JObject)newObject, subpath, rules);
                                    if (updatedObject.Item1)//if change detected
                                    {
                                        changeDetected = true;
                                        newJsonObject.Replace(updatedObject.Item2);
                                    }
                                }


                            }

                        }

                    }

                }

            }
            if (changedArray.Count > 0)
            {
                changeDetected = true;
                resultJson["changesDetected"] = JToken.FromObject(changedArray);
            }
            return new Tuple<bool, JObject>(changeDetected, (JObject)resultJson);

        }

        private static Dictionary<string, Dictionary<string, string>> ConfigureComparisonRules(JsonType type)
        {
            Dictionary<string, Dictionary<string, string>> response = new Dictionary<string, Dictionary<string, string>>();
            switch (type)
            {
                case JsonType.Product:
                    response = GetProductRules();
                    break;
                case JsonType.PublisherBooking:
                    response = GetPublisherBookingRules();
                    break;
                case JsonType.SubscriberBooking:
                    response = GetSubscriberBookingRules();
                    break;
            }

            return response;
            
        }

        private static Dictionary<string, Dictionary<string, string>> GetProductRules()
        {
            var rules = new Dictionary<string, Dictionary<string, string>>();



            rules["/"] = new Dictionary<string, string>() {
                                //idCol is not sent becasue not needed
                                {"ignoreList","id,name, additionalRegions" }
            };


            rules["/options"] = new Dictionary<string, string>() {
                                { "idCol", "id" },
                                {"ignoreList", "type"}
            };


            rules["/itineraryText"] = new Dictionary<string, string>() {
                                { "idCol", "id" }
            };
            //rules["/itineraryText/details"] = new Dictionary<string, string>() {
            //                    { "idCol", "detailid" }
            //};

            rules["/applicableRatings/types"] = new Dictionary<string, string>() {
                                { "idCol", "typeId" }
            };

            rules["/applicableRatings/types/ratings"] = new Dictionary<string, string>() {
                                { "idCol", "id" }
            };
            rules["/addresses"] = new Dictionary<string, string>() {
                                { "idCol", "addresstypeId" }
            };
            rules["/extras"] = new Dictionary<string, string>() {
                                { "idCol", "id" },
                                {"ignoreList", "type"}
            };

            rules["/facilities"] = new Dictionary<string, string>() {
                                { "idCol", "facilityId" }
            };

            rules["/notes"] = new Dictionary<string, string>() {
                                { "idCol", "id" }
            };

            rules["/options/dependentOptions"] = new Dictionary<string, string>() {
                                { "idCol", "id" }
            };

            rules["/extras/dependentOptions"] = new Dictionary<string, string>() {
                                { "idCol", "id" }
            };

            rules["/elements"] = new Dictionary<string, string>() {
                                { "idCol", "id" }
            };

            rules["/optionals"] = new Dictionary<string, string>() {
                                { "idCol", "id" }
            };

             rules["/termList"] = new Dictionary<string, string>() {
                                { "idCol", "termId" }
                          
            };

             rules["/packageItinerary"] = new Dictionary<string, string>() {
                                { "idCol", "Id" }
                          
            };
            return rules;
        }

        private static Dictionary<string, Dictionary<string, string>> GetPublisherBookingRules()
        {
            var rules = new Dictionary<string, Dictionary<string, string>>();



            rules["/"] = new Dictionary<string, string>() {
                                //idCol is not sent becasue not needed
                                {"ignoreList","id,name" }
            };


            rules["/BookedServices"] = new Dictionary<string, string>() {
                                { "idCol", "bookedServiceId" }
            };  

            rules["/BookedServices/BookedOptions"] = new Dictionary<string, string>() {
                                { "idCol", "BookedOptionId" }, {"ignoreList","AssignedPassengers" }
            };

            rules["/BookedServices/BookedOptions/Price"] = new Dictionary<string, string>() {
                                {"ignoreList","id,name" }
            };

            rules["/BookedServices/BookedOptions/BookedChildRates"] = new Dictionary<string, string>() {
                                { "idCol", "MpBookedChildRateId" }
            };

            rules["/BookedServices/BookedOptions/BookedChildRates/ChildPrice"] = new Dictionary<string, string>() {
                                {"ignoreList","id,name" }
            };

            rules["/BookingPassengers"] = new Dictionary<string, string>() {
                                { "idCol", "passengerId" }
            };

            rules["/BookedServices/PickupDropoffDetails"] = new Dictionary<string, string>() {
                                {"ignoreList","id,name" }
            };

            return rules;
        }

        private static Dictionary<string, Dictionary<string, string>> GetSubscriberBookingRules()
        {
            var rules = new Dictionary<string, Dictionary<string, string>>();



            rules["/"] = new Dictionary<string, string>() {
                                //idCol is not sent becasue not needed
                                {"ignoreList","id,name, Notes" }
            };


            
            rules["/BookedServices"] = new Dictionary<string, string>() {
                                { "idCol", "Mpbookedserviceid" }
            };  

            rules["/BookedServices/BookedOptions"] = new Dictionary<string, string>() {
                                { "idCol", "BookedOptionId" },  {"ignoreList","AssignedPassengers" }
            };

            rules["/BookingPassengers"] = new Dictionary<string, string>() {
                                { "idCol", "PassengerId" }
            };

            rules["/Notes"] = new Dictionary<string, string>() {
                                { "idCol", "NoteId" }
            };



            return rules;
        }

    }
}
