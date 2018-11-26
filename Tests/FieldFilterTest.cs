using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable ConvertToLocalFunction
// ReSharper disable InvokeAsExtensionMethod

namespace Tests
{
    [TestClass]
    public class FieldFilterTest
    {
        [NotNull]
        private RecordFieldCollection _GetFieldCollection()
        {
            return new RecordFieldCollection
            {
                new RecordField("100")
                    .AddSubField('a', "SubA")
                    .AddSubField('b', "SubB")
                    .AddSubField('c', "SubC")
                    .AddSubField('d', "SubD")
                    .AddSubField('e', "SubE"),
                new RecordField("101")
                    .AddSubField('f', "SubF")
                    .AddSubField('g', "SubG")
                    .AddSubField('h', "SubH")
                    .AddSubField('i', "SubI")
                    .AddSubField('j', "SubJ"),
                new RecordField("200")
                    .AddSubField('k', "SubK")
                    .AddSubField('l', "SubL")
                    .AddSubField('m', "SubM")
                    .AddSubField('n', "SubN")
                    .AddSubField('o', "SubO"),
                new RecordField("200")
                    .AddSubField('p', "SubP")
                    .AddSubField('q', "SubQ")
                    .AddSubField('r', "SubR")
                    .AddSubField('s', "SubS")
                    .AddSubField('t', "SubT")
            };
        }

        [NotNull]
        private IEnumerable<RecordField> _GetFieldEnumeration()
        {
            yield return new RecordField("100")
                .AddSubField('a', "SubA")
                .AddSubField('b', "SubB")
                .AddSubField('c', "SubC")
                .AddSubField('d', "SubD")
                .AddSubField('e', "SubE");
            yield return new RecordField("101")
                .AddSubField('f', "SubF")
                .AddSubField('g', "SubG")
                .AddSubField('h', "SubH")
                .AddSubField('i', "SubI")
                .AddSubField('j', "SubJ");
            yield return new RecordField("200")
                .AddSubField('k', "SubK")
                .AddSubField('l', "SubL")
                .AddSubField('m', "SubM")
                .AddSubField('n', "SubN")
                .AddSubField('o', "SubO");
            yield return new RecordField("200")
                .AddSubField('p', "SubP")
                .AddSubField('q', "SubQ")
                .AddSubField('r', "SubR")
                .AddSubField('s', "SubS")
                .AddSubField('t', "SubT");
        }

        [NotNull]
        [ItemNotNull]
        private SubField[] _GetSubFieldArray()
        {
            return new[]
            {
                new SubField('a', "SubA"),
                new SubField('b', "SubB"),
                new SubField('c', "SubC"),
                new SubField('d', "SubD"),
                new SubField('e', "SubE"),
            };
        }

        [NotNull]
        [ItemNotNull]
        private SubFieldCollection _GetSubFieldCollection()
        {
            return new SubFieldCollection
            {
                new SubField('a', "SubA"),
                new SubField('b', "SubB"),
                new SubField('c', "SubC"),
                new SubField('d', "SubD"),
                new SubField('e', "SubE"),
            };
        }

        [NotNull]
        [ItemNotNull]
        private IEnumerable<SubField> _GetSubFieldEnumeration()
        {
            yield return new SubField('a', "SubA1");
            yield return new SubField('b', "SubB1");
            yield return new SubField('c', "SubC1");
            yield return new SubField('a', "SubA2");
            yield return new SubField('b', "SubB2");
        }

        [TestMethod]
        public void FieldFilter_EmptyFieldArray_1()
        {
            Assert.AreEqual(0, FieldFilter.EmptyFieldArray.Length);
        }

        [TestMethod]
        public void FieldFilter_EmptySubFieldArray_1()
        {
            Assert.AreEqual(0, FieldFilter.EmptySubFieldArray.Length);
        }

        [TestMethod]
        public void FieldFilter_AllSubFields_1()
        {
            IEnumerable<RecordField> fields = _GetFieldEnumeration();
            SubField[] subFields = FieldFilter.AllSubFields(fields);
            Assert.AreEqual(20, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_01()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = FieldFilter.GetField(enumeration, "100");
            Assert.AreEqual(1, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, "200");
            Assert.AreEqual(2, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, "300");
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_02()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            RecordField[] fields = FieldFilter.GetField(collection, "100");
            Assert.AreEqual(1, fields.Length);

            fields = FieldFilter.GetField(collection, "200");
            Assert.AreEqual(2, fields.Length);

            fields = FieldFilter.GetField(collection, "300");
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_03()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField field = FieldFilter.GetField(enumeration, "100", 0);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetField(enumeration, "100", 1);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetField(enumeration, "200", 0);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetField(enumeration, "200", 1);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetField(enumeration, "200", 2);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetField(enumeration, "300", 1);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void FieldFilter_GetField_04()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            RecordField field = FieldFilter.GetField(collection, "100", 0);
            Assert.IsNotNull(field);

            field = FieldFilter.GetField(collection, "100", 1);
            Assert.IsNull(field);

            field = FieldFilter.GetField(collection, "200", 0);
            Assert.IsNotNull(field);

            field = FieldFilter.GetField(collection, "200", 1);
            Assert.IsNotNull(field);

            field = FieldFilter.GetField(collection, "200", 2);
            Assert.IsNull(field);

            field = FieldFilter.GetField(collection, "300", 1);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void FieldFilter_GetField_05()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] tags = { "100", "200" };
            RecordField[] fields = FieldFilter.GetField(enumeration, tags);
            Assert.AreEqual(3, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "200", "300" };
            fields = FieldFilter.GetField(enumeration, tags);
            Assert.AreEqual(2, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "300", "400" };
            fields = FieldFilter.GetField(enumeration, tags);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_06()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            string[] tags = { "100", "200" };
            RecordField[] fields = FieldFilter.GetField(collection, tags);
            Assert.AreEqual(3, fields.Length);

            tags = new[] { "200", "300" };
            fields = FieldFilter.GetField(collection, tags);
            Assert.AreEqual(2, fields.Length);

            tags = new[] { "300", "400" };
            fields = FieldFilter.GetField(collection, tags);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_07()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] tags = { "100", "200" };
            RecordField field = FieldFilter.GetField(enumeration, tags, 0);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetField(enumeration, tags, 1);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetField(enumeration, tags, 2);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetField(enumeration, tags, 3);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "300", "400" };
            field = FieldFilter.GetField(enumeration, tags, 0);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void FieldFilter_GetField_08()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            string[] tags = { "100", "200" };
            RecordField field = FieldFilter.GetField(collection, tags, 0);
            Assert.IsNotNull(field);

            field = FieldFilter.GetField(collection, tags, 1);
            Assert.IsNotNull(field);

            field = FieldFilter.GetField(collection, tags, 2);
            Assert.IsNotNull(field);

            field = FieldFilter.GetField(collection, tags, 3);
            Assert.IsNull(field);

            tags = new[] { "300", "400" };
            field = FieldFilter.GetField(collection, tags, 0);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void FieldFilter_GetField_09()
        {
            Func<RecordField, bool> predicate = field => field.Tag == "100";
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = FieldFilter.GetField(enumeration, predicate);
            Assert.AreEqual(1, fields.Length);

            predicate = field => field.Tag == "200";
            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, predicate);
            Assert.AreEqual(2, fields.Length);

            predicate = field => field.Tag == "300";
            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, predicate);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_10()
        {
            Func<SubField, bool> predicate = sub => sub.Text.Contains("A");
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = FieldFilter.GetField(enumeration, predicate);
            Assert.AreEqual(1, fields.Length);

            predicate = sub => sub.Text.Contains("Z");
            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, predicate);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_11()
        {
            Func<SubField, bool> predicate = sub => sub.Text.Contains("Sub");
            char[] codes = { 'a', 'b' };
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = FieldFilter.GetField(enumeration, codes, predicate);
            Assert.AreEqual(1, fields.Length);

            predicate = sub => sub.Text.Contains("Zub");
            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, codes, predicate);
            Assert.AreEqual(0, fields.Length);

            predicate = sub => sub.Text.Contains("Sub");
            codes = new[] { 'x', 'z' };
            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, codes, predicate);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_12()
        {
            char[] codes = { 'a', 'b' };
            string[] values = { "SubA", "SubB" };
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = FieldFilter.GetField(enumeration, codes, values);
            Assert.AreEqual(1, fields.Length);

            codes = new[] { 'x', 'z' };
            values = new[] { "SubA", "SubB" };
            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, codes, values);
            Assert.AreEqual(0, fields.Length);

            codes = new[] { 'a', 'b' };
            values = new[] { "SubX", "SubZ" };
            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, codes, values);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_13()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = FieldFilter.GetField(enumeration, 'a', "SubA");
            Assert.AreEqual(1, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, 'a', "SubB");
            Assert.AreEqual(0, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetField(enumeration, 'b', "SubA");
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_14()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] tags = { "100", "200" };
            char[] codes = { 'a', 'b' };
            string[] values = { "SubA", "SubB" };
            RecordField[] fields = FieldFilter.GetField(enumeration, tags, codes, values);
            Assert.AreEqual(1, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "200", "300" };
            codes = new[] { 'a', 'b' };
            values = new[] { "SubA", "SubB" };
            fields = FieldFilter.GetField(enumeration, tags, codes, values);
            Assert.AreEqual(0, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "200" };
            codes = new[] { 'x', 'z' };
            values = new[] { "SubA", "SubB" };
            fields = FieldFilter.GetField(enumeration, tags, codes, values);
            Assert.AreEqual(0, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "200" };
            codes = new[] { 'a', 'b' };
            values = new[] { "SubX", "SubZ" };
            fields = FieldFilter.GetField(enumeration, tags, codes, values);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetField_15()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Func<RecordField, bool> fieldPredicate = field => field.Tag == "100";
            Func<SubField, bool> subPredicate = sub => sub.Code == 'a';
            RecordField[] fields = FieldFilter.GetField(enumeration, fieldPredicate, subPredicate);
            Assert.AreEqual(1, fields.Length);

            enumeration = _GetFieldEnumeration();
            fieldPredicate = field => field.Tag == "200";
            subPredicate = sub => sub.Code == 'a';
            fields = FieldFilter.GetField(enumeration, fieldPredicate, subPredicate);
            Assert.AreEqual(0, fields.Length);

            enumeration = _GetFieldEnumeration();
            fieldPredicate = field => field.Tag == "100";
            subPredicate = sub => sub.Code == 'z';
            fields = FieldFilter.GetField(enumeration, fieldPredicate, subPredicate);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetFieldRegex_1()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = FieldFilter.GetFieldRegex(enumeration, "^1");
            Assert.AreEqual(2, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetFieldRegex(enumeration, "^2");
            Assert.AreEqual(2, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = FieldFilter.GetFieldRegex(enumeration, "^3");
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetFieldRegex_2()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField field = FieldFilter.GetFieldRegex(enumeration, "^1", 0);
            Assert.IsNotNull(field);
            Assert.AreEqual("100", field.Tag);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetFieldRegex(enumeration, "^1", 1);
            Assert.IsNotNull(field);
            Assert.AreEqual("101", field.Tag);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetFieldRegex(enumeration, "^1", 2);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetFieldRegex(enumeration, "^2", 0);
            Assert.IsNotNull(field);
            Assert.AreEqual("200", field.Tag);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetFieldRegex(enumeration, "^2", 2);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            field = FieldFilter.GetFieldRegex(enumeration, "^3", 0);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void FieldFilter_GetFieldRegex_3()
        {
            IEnumerable<RecordField> enumeration = new[]
            {
                new RecordField("100", "This is a text"),
                new RecordField("100", "That is a text"),
                new RecordField("101", "Another text"),
                new RecordField("200", "This is a text"),
            };
            string[] tags = { "100", "101" };
            RecordField[] fields = FieldFilter.GetFieldRegex(enumeration, tags, "^This");
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetFieldRegex_4()
        {
            RecordField[] enumeration =
            {
                new RecordField("100", "This is a text"),
                new RecordField("100", "That is a text"),
                new RecordField("101", "Another text"),
                new RecordField("200", "This is a text"),
            };
            string[] tags = { "100", "101" };
            RecordField field = FieldFilter.GetFieldRegex(enumeration, tags, "^This", 0);
            Assert.IsNotNull(field);
            Assert.AreEqual("100", field.Tag);

            field = FieldFilter.GetFieldRegex(enumeration, tags, "^This", 1);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void FieldFilter_GetFieldRegex_5()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] tags = { "100", "101" };
            char[] codes = { 'a', 'f' };
            RecordField[] fields = FieldFilter.GetFieldRegex(enumeration, tags, codes, "^Sub");
            Assert.AreEqual(2, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "200", "201" };
            codes = new[] { 'a', 'f' };
            fields = FieldFilter.GetFieldRegex(enumeration, tags, codes, "^Sub");
            Assert.AreEqual(0, fields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "101" };
            codes = new[] { 'a', 'z' };
            fields = FieldFilter.GetFieldRegex(enumeration, tags, codes, "^Dub");
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetFieldRegex_6()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] tags = { "100", "101" };
            char[] codes = { 'a', 'f' };
            RecordField field = FieldFilter.GetFieldRegex(enumeration, tags, codes, "^Sub", 0);
            Assert.IsNotNull(field);
            Assert.AreEqual("100", field.Tag);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "101" };
            codes = new[] { 'a', 'f' };
            field = FieldFilter.GetFieldRegex(enumeration, tags, codes, "^Sub", 1);
            Assert.IsNotNull(field);
            Assert.AreEqual("101", field.Tag);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "101" };
            codes = new[] { 'a', 'f' };
            field = FieldFilter.GetFieldRegex(enumeration, tags, codes, "^Sub", 2);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "101" };
            codes = new[] { 'a', 'f' };
            field = FieldFilter.GetFieldRegex(enumeration, tags, codes, "^Dub", 0);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void FieldFilter_GetFieldText_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField("100", "This is a text"),
                new RecordField("100"),
                new RecordField("101", "Another text"),
                new RecordField("200", "This is a text")
            };

            string[] text = FieldFilter.GetFieldText(collection);
            Assert.AreEqual(3, text.Length);
        }

        [TestMethod]
        public void FieldFilter_GetFieldText_2()
        {
            string text = "This is a field";
            RecordField field = new RecordField("100", text);
            Assert.AreEqual(text, FieldFilter.GetFieldText(field));

            field = new RecordField("100");
            Assert.IsNull(FieldFilter.GetFieldText(field));

            field = null;
            Assert.IsNull(FieldFilter.GetFieldText(field));
        }

        [TestMethod]
        public void FieldFilter_GetFieldText_3()
        {
            RecordField[] fields =
            {
                new RecordField("100", "This is a text"),
                new RecordField("100"),
                new RecordField("101", "Another text"),
                new RecordField("200", "This is a text"),
            };
            string[] text = FieldFilter.GetFieldText(fields);
            Assert.AreEqual(3, text.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_1()
        {
            IEnumerable<SubField> enumeration = _GetSubFieldEnumeration();
            char[] codes = { 'a', 'b' };
            SubField[] subFields = FieldFilter.GetSubField(enumeration, codes);
            Assert.AreEqual(4, subFields.Length);

            enumeration = _GetSubFieldEnumeration();
            codes = new[] { 'x', 'z' };
            subFields = FieldFilter.GetSubField(enumeration, codes);
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_2()
        {
            SubFieldCollection collection = _GetSubFieldCollection();
            char[] codes = { 'a', 'b' };
            SubField[] subFields = FieldFilter.GetSubField(collection, codes);
            Assert.AreEqual(2, subFields.Length);

            codes = new[] { 'x', 'z' };
            subFields = FieldFilter.GetSubField(collection, codes);
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_3()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            char[] codes = { 'a', 'b' };
            SubField[] subFields = FieldFilter.GetSubField(enumeration, codes);
            Assert.AreEqual(2, subFields.Length);

            enumeration = _GetFieldEnumeration();
            codes = new[] { 'x', 'z' };
            subFields = FieldFilter.GetSubField(enumeration, codes);
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_4()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            SubField[] subFields = FieldFilter.GetSubField(enumeration, "100", 'a');
            Assert.AreEqual(1, subFields.Length);

            enumeration = _GetFieldEnumeration();
            subFields = FieldFilter.GetSubField(enumeration, "100", 'z');
            Assert.AreEqual(0, subFields.Length);

            enumeration = _GetFieldEnumeration();
            subFields = FieldFilter.GetSubField(enumeration, "300", 'a');
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_5()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            SubField subField = FieldFilter.GetSubField(enumeration, "100", 0, 'a', 0);
            Assert.IsNotNull(subField);
            Assert.AreEqual('a', subField.Code);

            enumeration = _GetFieldEnumeration();
            subField = FieldFilter.GetSubField(enumeration, "100", 1, 'a', 0);
            Assert.IsNull(subField);

            enumeration = _GetFieldEnumeration();
            subField = FieldFilter.GetSubField(enumeration, "100", 0, 'a', 1);
            Assert.IsNull(subField);

            enumeration = _GetFieldEnumeration();
            subField = FieldFilter.GetSubField(enumeration, "300", 0, 'a', 0);
            Assert.IsNull(subField);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_6()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            SubField subField = FieldFilter.GetSubField(enumeration, "100", 'a', 0);
            Assert.IsNotNull(subField);
            Assert.AreEqual('a', subField.Code);

            enumeration = _GetFieldEnumeration();
            subField = FieldFilter.GetSubField(enumeration, "100", 'a', 1);
            Assert.IsNull(subField);

            enumeration = _GetFieldEnumeration();
            subField = FieldFilter.GetSubField(enumeration, "300", 'a', 0);
            Assert.IsNull(subField);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_7()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            SubField[] subFields = FieldFilter.GetSubField(enumeration, 'a');
            Assert.AreEqual(1, subFields.Length);

            enumeration = _GetFieldEnumeration();
            subFields = FieldFilter.GetSubField(enumeration, 'z');
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_8()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Func<RecordField, bool> fieldPredicate =
                field => field.Tag.ThrowIfNull().StartsWith("1");
            Func<SubField, bool> subPredicate = sub => sub.Code < 'i';
            SubField[] subFields = FieldFilter.GetSubField(enumeration, fieldPredicate, subPredicate);
            Assert.AreEqual(8, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubField_9()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] tags = { "100", "101" };
            char[] codes = { 'a', 'b' };
            SubField[] subFields = FieldFilter.GetSubField(enumeration, tags, codes);
            Assert.AreEqual(2, subFields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "300", "301" };
            codes = new[] { 'a', 'b' };
            subFields = FieldFilter.GetSubField(enumeration, tags, codes);
            Assert.AreEqual(0, subFields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "101" };
            codes = new[] { 'x', 'z' };
            subFields = FieldFilter.GetSubField(enumeration, tags, codes);
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubFieldRegex_1()
        {
            SubField[] array = _GetSubFieldArray();
            SubField[] subFields = FieldFilter.GetSubFieldRegex(array, "[abc]");
            Assert.AreEqual(3, subFields.Length);

            subFields = FieldFilter.GetSubFieldRegex(array, "[xyz]");
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubFieldRegex_2()
        {
            SubField[] array = _GetSubFieldArray();
            char[] codes = { 'a', 'b' };
            SubField[] subFields = FieldFilter.GetSubFieldRegex(array, codes, "^Sub");
            Assert.AreEqual(2, subFields.Length);

            codes = new[] { 'x', 'z' };
            subFields = FieldFilter.GetSubFieldRegex(array, codes, "^Sub");
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubFieldRegex_3()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] tags = { "100", "101" };
            char[] codes = { 'a', 'b' };
            SubField[] subFields = FieldFilter.GetSubFieldRegex(enumeration, tags, codes, "^Sub");
            Assert.AreEqual(2, subFields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "300", "301" };
            codes = new[] { 'a', 'b' };
            subFields = FieldFilter.GetSubFieldRegex(enumeration, tags, codes, "^Sub");
            Assert.AreEqual(0, subFields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "101" };
            codes = new[] { 'x', 'z' };
            subFields = FieldFilter.GetSubFieldRegex(enumeration, tags, codes, "^Sub");
            Assert.AreEqual(0, subFields.Length);

            enumeration = _GetFieldEnumeration();
            tags = new[] { "100", "101" };
            codes = new[] { 'a', 'b' };
            subFields = FieldFilter.GetSubFieldRegex(enumeration, tags, codes, "^Dub");
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubFieldText_1()
        {
            string text = "This is a text";
            SubField subField = new SubField('a', text);
            Assert.AreEqual(text, FieldFilter.GetSubFieldText(subField));

            subField = new SubField('a');
            Assert.IsNull(FieldFilter.GetSubFieldText(subField));

            subField = null;
            Assert.IsNull(FieldFilter.GetSubFieldText(subField));
        }

        [TestMethod]
        public void FieldFilter_GetSubFieldText_2()
        {
            SubField[] subFields =
            {
                new SubField('a', "This is a text"),
                new SubField('b'),
                new SubField('c', "Text too"),
                new SubField('d', "Text again")
            };
            string[] text = FieldFilter.GetSubFieldText(subFields);
            Assert.AreEqual(3, text.Length);
        }

        [TestMethod]
        public void FieldFilter_GetSubFieldText_3()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string text = FieldFilter.GetSubFieldText(enumeration, "100", 'a');
            Assert.AreEqual("SubA", text);

            enumeration = _GetFieldEnumeration();
            text = FieldFilter.GetSubFieldText(enumeration, "100", 'z');
            Assert.IsNull(text);

            enumeration = _GetFieldEnumeration();
            text = FieldFilter.GetSubFieldText(enumeration, "300", 'a');
            Assert.IsNull(text);
        }
    }
}
