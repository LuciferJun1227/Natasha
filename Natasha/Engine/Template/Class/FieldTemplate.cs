﻿using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class FieldTemplate<T>:InheritanceTemplate<T>
    {
        public readonly StringBuilder FieldScript;
        internal readonly HashSet<string> _fieldsSet;

        public FieldTemplate()
        {
            FieldScript = new StringBuilder();
            _fieldsSet = new HashSet<string>();
        }
        public T Field(string level, Type type, string name)
        {
            Using(type);
            if (!_fieldsSet.Contains(name))
            {
                FieldScript.Append($"{level} {NameReverser.GetName(type)} {name};");
            }
            return Link;
        }

        public override string Builder()
        {
            Script.Append(FieldScript);
            return base.Builder();
        }

        public T PublicField<S>(string name)
        {
            return PublicField(typeof(S), name);
        }

        public T PublicField(Type type, string name)
        {
            return Field("public", type, name);
        }

        public T PrivateField<S>(string name)
        {
            return PrivateField(typeof(S), name);
        }

        public T PrivateField(Type type, string name)
        {
            return Field("private", type, name);
        }

        public T InternalField<S>(string name)
        {
            return InternalField(typeof(S), name);
        }

        public T InternalField(Type type, string name)
        {
            return Field("internal", type, name);
        }

        public T ProtectedField<S>(string name)
        {
            return ProtectedField(typeof(S), name);
        }

        public T ProtectedField(Type type, string name)
        {
            return Field("protected", type, name);
        }

        public T PublicStaticField<S>(string name)
        {
            return PublicStaticField(typeof(S), name);
        }

        public T PublicStaticField(Type type, string name)
        {
            return Field("public static", type, name);
        }

        public T PrivateStaticField<S>(string name)
        {
            return PrivateStaticField(typeof(S), name);
        }

        public T PrivateStaticField(Type type, string name)
        {
            return Field("private static", type, name);
        }

        public T InternalStaticField<S>(string name)
        {
            return InternalStaticField(typeof(S), name);
        }

        public T InternalStaticField(Type type, string name)
        {
            return Field("internal static", type, name);
        }

        public T ProtectedStaticField<S>(string name)
        {
            return ProtectedStaticField(typeof(S), name);
        }

        public T ProtectedStaticField(Type type, string name)
        {
            return Field("protected static", type, name);
        }
    }
}
