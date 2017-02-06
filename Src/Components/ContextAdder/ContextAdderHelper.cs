using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;

namespace BizTalk.PipelineComponents
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("8fc58bc6-e6af-401d-901d-2d6d963c0d87"), CLSCompliant(false)]
    internal interface IBizTalkBuildSnapShot
    {
        object GetCompilableFiles();
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object GetCommonProjectProperties();
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object GetConfigProperties();
        object GetReferences();
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetTempAssemblyPath();
    }

    public class ContextValueCollectionEditor : System.Drawing.Design.UITypeEditor
    {
        public ContextValueCollectionEditor()
        {
            _editor = new CollectionEditor(typeof(ContextValueCollection));

        }
        CollectionEditor _editor;
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return _editor.GetEditStyle(context);
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            try
            {
                Type t = context.Instance.GetType();
                FieldInfo fi = t.GetField("pipelineFileComponentInfo", BindingFlags.Instance | BindingFlags.NonPublic);
                object ci = fi.GetValue(context.Instance);
                Type t2 = ci.GetType();
                PropertyInfo dirty = t2.GetProperty("Dirty", BindingFlags.Instance | BindingFlags.NonPublic);
                dirty.SetValue(ci, true, null);
            }
            catch
            { }


            return _editor.EditValue(context, provider, value);
        }


    }
    public delegate void IsDirty();

    [System.ComponentModel.Editor("Common.BizTalk.PipelineComponents.ContextAdderHelper.ContextValueCollectionEditor", typeof(System.Drawing.Design.UITypeEditor))]
    public class ContextValueCollection : System.Collections.CollectionBase
    {

        private Dictionary<string, string> namespaces = new Dictionary<string, string>();
        string ns = String.Empty;

        public  ContextValueCollection()
        {
            namespaces.Add("BTS", "http://schemas.microsoft.com/BizTalk/2003/system-properties");
            namespaces.Add("FILE", "http://schemas.microsoft.com/BizTalk/2003/file-properties");
            namespaces.Add("XMLNORM", "http://schemas.microsoft.com/BizTalk/2003/xmlnorm-properties");
        }


        protected override void OnInsert(int index, object value)
        {
            FireDirty();
            ContextValue cv = (ContextValue)value;
            cv.Dirty += new IsDirty(this.FireDirty);
            base.OnInsert(index, value);

           
        }

        void FireDirty()
        {
            if (Dirty != null)
                Dirty();
        }
        public event IsDirty Dirty;

        public ContextValue this[int index]
        {
            get { return (ContextValue)this.InnerList[index]; }
            set { this.InnerList[index] = value; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            
            foreach (ContextValue context in this)
            {
                if (namespaces.TryGetValue(context.Namespace, out ns) == false)
                    ns = context.Namespace;

                builder.AppendFormat("{0}#{1}={2};", ns, context.Key, context.Value);
            }
            
            return builder.ToString();

        }

        public void SetCollection(string stringCollection)
        {
            string[] arrCollection = stringCollection.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            this.InnerList.Clear();
            

            foreach (string coll in arrCollection)
	        {
                
                string[] context = coll.Split(new char[] {'#','='}, StringSplitOptions.RemoveEmptyEntries);

                if (namespaces.TryGetValue(context[0], out ns) == false)
                    ns = context[0];

                this.InnerList.Add(new ContextValue { Key = context[1], Namespace = ns, Value = context[2] });
	        }
            
        }
       

    }
    public class ContextValue
    {
        internal event IsDirty Dirty;
        void LocalDirty()
        {
            if (Dirty != null)
                Dirty();
        }
       
        public string Value
        {
            get { return _Value; }
            set
            {
                LocalDirty();
                _Value = value;
            }
        }
        public string Key
        {
            get { return _Key; }
            set
            {
                LocalDirty();
                _Key = value;
            }
        }
        public string Namespace
        {
            get { return _Namespace; }
            set
            {
                LocalDirty();
                _Namespace = value;
            }
        }

        
        string _Namespace;
        string _Value;
        string _Key;
  
    }
}
