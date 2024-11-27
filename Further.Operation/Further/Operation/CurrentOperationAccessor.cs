using FluentResults;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using YamlDotNet.Core.Tokens;

namespace Further.Operation
{
    public class CurrentOperationAccessor : ICurrentOperationAccessor
    {
        public static CurrentOperationAccessor Instance { get; } = new();
        private readonly AsyncLocal<BasicOperationInfo?> _currentScope;
        public BasicOperationInfo? Current
        {
            get { return _currentScope.Value; }
            set
            {
                if (_currentScope.Value != null && value != null)
                {
                    _currentScope.Value.Name = value.Name;
                    _currentScope.Value.Result = value.Result;
                    _currentScope.Value.Correlations = value.Correlations;
                }
            }
        }

        public CurrentOperationAccessor()
        {
            _currentScope = new AsyncLocal<BasicOperationInfo?>();
        }

        public void Initial(Guid id)
        {
            if (this._currentScope.Value == null)
                this._currentScope.Value = new BasicOperationInfo(id);
        }
    }

}
