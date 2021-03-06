﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VW;

namespace cs_unittest
{
    [TestClass]
    public class TestJsonClass
    {
        [TestMethod]
        public void TestJson()
        {
            using (var validator = new VowpalWabbitExampleJsonValidator())
            {
                validator.Validate("|a foo:1", "{\"a\":{\"foo\":1}}");
                validator.Validate("|a foo:2.3", "{\"a\":{\"foo\":2.3}}");
                validator.Validate("|a foo:2.3 bar", "{\"a\":{\"foo\":2.3, \"bar\":true}}");
                validator.Validate("|a foo:1 |bcd 25_old", "{\"a\":{\"foo\":1},\"bcd\":{\"Age\":\"25 old\"}}");
            }
        }

        [TestMethod]
        public void TestJsonAux()
        {
            using (var validator = new VowpalWabbitExampleJsonValidator())
            {
                validator.Validate("|a foo:1", "{\"a\":{\"foo\":1},\"_aux\":{\"abc\":{\"def\":3}}}");
                validator.Validate("|a foo:1", "{\"a\":{\"foo\":1},\"_aux\":5}");
                validator.Validate("|a foo:1", "{\"a\":{\"foo\":1},\"_aux\":[1,2,[3,4],2]}");
            }
        }

        [TestMethod]
        public void TestJsonArray()
        {
            using (var validator = new VowpalWabbitExampleJsonValidator())
            {
                validator.Validate("| :1 :2.3 :4", "{\"a\":[1,2.3,4]}");
                validator.Validate("|a :1 :2.3 :4", "{\"a\":{\"b\":[1,2.3,4]}}");
            }
        }

        [TestMethod]
        public void TestJsonSimpleLabel()
        {
            using (var validator = new VowpalWabbitExampleJsonValidator())
            {
                validator.Validate("1 |a foo:1", "{\"_label\":{\"Label\":1},\"a\":{\"foo\":1}}", VowpalWabbitLabelComparator.Simple);
            }
        }

        [TestMethod]
        public void TestJsonVWLabel()
        {
            using (var validator = new VowpalWabbitExampleJsonValidator())
            {
                validator.Validate("1 |a foo:1", "{\"_label\":1,\"a\":{\"foo\":1}}", VowpalWabbitLabelComparator.Simple);
                validator.Validate("1 |a foo:1", "{\"_label\":\"1\",\"a\":{\"foo\":1}}", VowpalWabbitLabelComparator.Simple);
            }
        }

        [TestMethod]
        public void TestJsonContextualBanditLabel()
        {
            using (var validator = new VowpalWabbitExampleJsonValidator("--cb 2 --cb_type dr"))
            {
                validator.Validate("1:-2:.3 |a foo:1",
                    "{\"_label\":{\"Action\":1,\"Cost\":-2,\"Probability\":.3},\"a\":{\"foo\":1}}",
                    VowpalWabbitLabelComparator.ContextualBandit);
                validator.Validate("1:2:.5 |a foo:1", "{\"_label\":\"1:2:.5\",\"a\":{\"foo\":1}}", VowpalWabbitLabelComparator.ContextualBandit);
            }
        }
    }
}
