//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MarketPlaceService.API.Utilities
//{
//    public class JsonUtility
//    {
//        public static JObject GetDiffJson(JObject oldJson, JObject newJson)
//        {
//            var rules = ConfigureComparisonRules();
//            return GetDiffJson(oldJson, newJson, "/", rules);
//        }

//        private static JObject GetDiffJson(JObject oldJson, JObject newJson, string currentPath, Dictionary<string, string> rules)
//        {
//            var resultJson = newJson.DeepClone();
//            List<ChangeDetected> changedArray = new List<ChangeDetected> { };

//            var allProperties = oldJson.Properties().Select(o => o.Name).Union(newJson.Properties().Select(o => o.Name));
//            foreach (var prop in allProperties)
//            {
//                var newpath = "";
//                if (!currentPath.EndsWith("/"))
//                    newpath = currentPath + "/" + prop;
//                else
//                    newpath = currentPath + prop;

//                if (!newJson.ContainsKey(prop))
//                {
//                    //property deleted
//                    changedArray.Add(new ChangeDetected
//                    {
//                        changeType = "Deleted",
//                        fieldName = prop,
//                        value = oldJson.Property(prop).Value.ToString()
//                    });
//                }
//                else if (!oldJson.ContainsKey(prop))
//                {//new object
//                    changedArray.Add(new ChangeDetected
//                    {
//                        changeType = "Added",
//                        fieldName = prop,
//                        value = ""
//                    });


//                }
//                else
//                { //change property
//                    if (oldJson.Property(prop).Value.ToString() != newJson.Property(prop).Value.ToString())
//                    {
//                        var oldVal = oldJson.Property(prop).Value;
//                        var newVal = newJson.Property(prop).Value;
//                        if (newVal.GetType() == typeof(JValue))
//                        {
//                            changedArray.Add(new ChangeDetected
//                            {
//                                changeType = "Changed",
//                                fieldName = prop,
//                                value = oldJson.Property(prop).Value.ToString()
//                            });
//                        }
//                        else if (newVal.GetType() == typeof(JObject))
//                        {
//                            var newobject = GetDiffJson((JObject)oldJson.Property(prop).Value, (JObject)newJson.Property(prop).Value, newpath, rules);
//                            resultJson[prop] = newobject;
//                        }
//                        else if (newVal.GetType() == typeof(JArray))
//                        {
//                            //


//                            var compareFieldName = rules[newpath];


//                            var recordExists = newVal.Select(o => o[compareFieldName]).Intersect(oldVal.Select(o => o[compareFieldName]));
//                            var insertRecord = newVal.Select(o => o[compareFieldName]).Except(oldVal.Select(o => o[compareFieldName]));
//                            var deletedRecord = oldVal.Select(o => o[compareFieldName]).Except(newVal.Select(o => o[compareFieldName]));

//                            if (deletedRecord.Count() > 0)
//                            {
//                                List<JObject> deletedchangedArrayList = new List<JObject>();
//                                foreach (var objRecord in deletedRecord)
//                                {
//                                    deletedchangedArrayList.Add((JObject)oldJson.Property(prop).Value.First(o => o[compareFieldName] == objRecord));
//                                }

//                                changedArray.Add(new ChangeDetected
//                                {
//                                    changeType = "Deleted",
//                                    fieldName = prop,
//                                    value = JToken.FromObject(deletedchangedArrayList).ToString()
//                                });
//                            }

//                            if (insertRecord.Count() > 0)
//                            {
//                                List<JObject> insertedchangedArrayList = new List<JObject>();
//                                foreach (var objRecord in insertRecord)
//                                {
//                                    insertedchangedArrayList.Add((JObject)newJson.Property(prop).Value.First(o => o[compareFieldName] == objRecord));
//                                }

//                                changedArray.Add(new ChangeDetected
//                                {
//                                    changeType = "Added",
//                                    fieldName = prop,
//                                    value = JToken.FromObject(insertedchangedArrayList).ToString()
//                                });
//                            }

//                            if (recordExists.Count() > 0)
//                            {
//                                foreach (var objRecord in recordExists)
//                                {
//                                    var oldObject = oldVal.First(o => o[compareFieldName].ToString().Equals(objRecord.ToString()));
//                                    var newObject = newVal.First(o => o[compareFieldName].ToString().Equals(objRecord.ToString()));
//                                    JArray newarray = (JArray)resultJson[prop];
//                                    var newJsonObject = newarray.First(o => o[compareFieldName].ToString().Equals(objRecord.ToString()));

//                                    var updatedObject = GetDiffJson((JObject)oldObject, (JObject)newObject, newpath, rules);
//                                    newJsonObject.Replace(updatedObject);
//                                }


//                            }

//                        }

//                    }

//                }

//            }
//            if (changedArray.Count > 0)
//            {
//                resultJson["ChangeDetected"] = JToken.FromObject(changedArray);
//            }
//            return (JObject)resultJson;

//        }

//        private static Dictionary<string, string> ConfigureComparisonRules()
//        {
//            var rules = new Dictionary<string, string>();
//            rules["/options"] = "optionid";
//            rules["/itineraryText"] = "id";
//            rules["/itineraryText/details"] = "detailid";
//            rules["/applicableRatings/types"] = "typeId";
//            rules["/applicableRatings/types/ratings"] = "id";
//            rules["/addresses"] = "addresstypeId";

//            return rules;
//        }

//    }
//}
