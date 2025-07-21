using System;
using System.Collections.Generic;

namespace NhaSachMetMoi.Models.Interpreter
{
    public class QueryInterpreter
    {
        public IExpression BuildChain(string query)
        {
            var keywords = query.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            IExpression head = null;
            IExpression current = null;

            foreach (var keyword in keywords)
            {
                var expr = new DefaultExpression(keyword);
                if (head == null)
                {
                    head = expr;
                    current = expr;
                }
                else
                {
                    current = current.SetNext(expr);
                }
            }

            return head;
        }
    }
}
