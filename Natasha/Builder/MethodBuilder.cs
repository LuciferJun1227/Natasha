﻿using Natasha;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha
{
    public class MethodBuilder<T> :IComplier
    {
        //private readonly static Regex _get_class;
        protected T _link;
        static MethodBuilder()
        {
            //_get_class = new Regex(@"\sclass.*?(?<result>[a-zA-Z0-9]*?)[\s]*{", RegexOptions.Compiled | RegexOptions.Singleline);
        }
        public static Action<string> SingleError;
        internal (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) _info;
        public ClassBuilder ClassTemplate;
        public MethodTemplate MethodTemplate;
        public MethodBuilder()
        {
            ClassTemplate = new ClassBuilder();
            MethodTemplate = new MethodTemplate();
        }
        
        public string MethodScript
        {
            get { return _info.Script; }
        }

        public virtual T UseClassTemplate(ClassBuilder template)
        {
            ClassTemplate = template;
            return _link;
        }

        public virtual T UseClassTemplate(Action<ClassBuilder> template)
        {
            template(ClassTemplate);
            return _link;
        }

        public virtual T UseBodyTemplate(MethodTemplate template)
        {
            MethodTemplate = template;
            return _link;
        }
        public virtual T UseBodyTemplate(Action<MethodTemplate> template)
        {
            template(MethodTemplate);
            return _link;
        }



        public override Delegate Complie()
        {
            _info = MethodTemplate.Package();
            ClassTemplate.Using(_info.Types);
            string script = ClassTemplate.Body(_info.Script).Builder();
            Assembly assembly = null;
            if (!_useFileComplie)
            {
                assembly = ScriptComplier.StreamComplier(script, ClassTemplate.NameScript, SingleError);
            }
            else
            {
                assembly = ScriptComplier.FileComplier(script, ClassTemplate.NameScript, SingleError);
            }


            if (assembly == null)
            {
                return null;
            }

            return AssemblyOperator
                .Loader(assembly)[ClassTemplate.NameScript]
                .GetMethod(_info.Flag)
                .CreateDelegate(_info.Delegate);
        }

    }
}
