using DictionaryOfWords.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service
{
    public class EntityOperationResult<T> where T : EntityBase
    {
        private EntityOperationResult(T entity)
        {
            Entity = entity;
        }

        private EntityOperationResult()
        {
        }
        public bool IsSuccess { get; private set; }

        public T Entity { get; }

        public string[] Errors { get; private set; }


        public static EntityOperationResult<T> Success(T entity)
        {
            var result = new EntityOperationResult<T>(entity);
            result.IsSuccess = true;
            return result;
        }

        public static EntityOperationResult<T> Failure()
        {
            var result = new EntityOperationResult<T>();
            result.IsSuccess = false;
            result.Errors = new string[0];

            return result;
        }

        public EntityOperationResult<T> AddError(params string[] errorMessages)
        {
            if (errorMessages?.Length > 0)
            {
                Errors = errorMessages;
            }
            else
            {
                Errors = new string[0];
            }
            return this;
        }
    }
}
