using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace PageRank
{
    /// <summary>
    ///
    /// </summary>
    internal class DeepCloneHelper
    {
        #region Privat Properties

        private const BindingFlags Binding = BindingFlags.Instance |
        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        private readonly object _desireObjectToBeCloned;
        private Dictionary<object, object> _visited;

        #endregion Privat Properties

        #region Contructure

        public DeepCloneHelper(object desireObjectToBeCloned)
        {
            if (desireObjectToBeCloned == null)
                throw new Exception("The desire object to be cloned cant be NULL");
            _desireObjectToBeCloned = desireObjectToBeCloned;
            _visited = new Dictionary<object, object>();
        }

        #endregion Contructure

        #region public Method Clone

        public object Clone()
        {
            return (object)DeepCopy<object>(_desireObjectToBeCloned);
        }

        /// <summary>
        /// Get the deep clone of an object.
        /// </summary>
        /// <returns>Return the deep clone.</returns>
        public T Clone<T>()
        {
            return (T)DeepCopy<T>(_desireObjectToBeCloned);
            //return (T)DeepClone(_desireObjectToBeCloned);
        }

        #endregion public Method Clone

        public T DeepCopy<T>(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// The method implements deep clone using reflection.
        /// </summary>
        /// <param name="objectToBeCloned">It is the object used to deep clone.</param>
        /// <returns>Return the deep clone.</returns>
        private object DeepClone(Object objectToBeCloned)
        {
            if (objectToBeCloned == null)
            {
                return null;
            }

            Type _primaryType = objectToBeCloned.GetType();

            Console.WriteLine(_primaryType);

            // If the type of object is the value type, we will always get a new object when
            // the original object is assigned to another variable. So if the type of the
            // object is primitive or enum, we just return the object. We will process the
            // struct type subsequently because the struct type may contain the reference
            // fields.
            // If the string variables contain the same chars, they always refer to the same
            // string in the heap. So if the type of the object is string, we also return the
            // object.
            if (objectToBeCloned == null)
                return null;

            if (_primaryType.IsPrimitive || _primaryType.IsEnum || _primaryType == typeof(string))
            {
                return objectToBeCloned;
            }

            // if object objectToBeCloned is visited then return the copied object from dictionary
            if (_visited.ContainsKey(objectToBeCloned)) return _visited[objectToBeCloned];

            // If the type of the object is the Array, we use the CreateInstance method to get
            // a new instance of the array. We also process recursively this method in the
            // elements of the original array because the type of the element may be the reference
            // type.
            else if (_primaryType.IsArray)
            {
                Type typeElement = Type.GetType(_primaryType.FullName.Replace("[]", string.Empty));
                var array = objectToBeCloned as Array;
                Array copiedArray = Array.CreateInstance(typeElement, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    // Get the deep clone of the element in the original array and assign the
                    // clone to the new array.
                    copiedArray.SetValue(DeepClone(array.GetValue(i)), i);
                }

                //adding in visited array
                _visited.Add(objectToBeCloned, copiedArray);

                return copiedArray;
            }
            // If the type of the object is class or struct, it may contain the reference fields,
            // so we use reflection and process recursively this method in the fields of the object
            // to get the deep clone of the object.
            // We use Type.IsValueType method here because there is no way to indicate directly whether
            // the Type is a struct type.
            else if (_primaryType.IsClass || _primaryType.IsValueType)
            {
                object copiedObject = Activator.CreateInstance(objectToBeCloned.GetType());

                // Get all FieldInfo.
                FieldInfo[] fields = _primaryType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(objectToBeCloned);
                    if (fieldValue != null)
                    {
                        var clonedFieldValue = DeepClone(fieldValue);
                        // Get the deep clone of the field in the original object and assign the
                        // clone to the field in the new object.
                        field.SetValue(copiedObject, clonedFieldValue);
                    }
                }

                //adding in visited array
                _visited.Add(objectToBeCloned, copiedObject);

                return copiedObject;
            }
            else
            {
                throw new ArgumentException("The object is unknown type");
            }
        }
    }
}