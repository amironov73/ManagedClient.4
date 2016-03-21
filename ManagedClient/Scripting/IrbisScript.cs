/* IrbisScript.cs
 */

#region Using directives

using System;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Scripting
{
    /// <summary>
    /// Интерпретатор Lua-скриптов с учётом ИРБИС-специфики.
    /// </summary>
    public sealed class IrbisScript
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Клиент для доступа к серверу
        /// </summary>
        public ManagedClient64 Client { get; private set; }

        /// <summary>
        /// Скриптовый движок.
        /// </summary>
        public Script Engine { get; private set; }

        /// <summary>
        /// Текущая запись
        /// </summary>
        public IrbisRecord Record { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public IrbisScript()
        {
            Client = new ManagedClient64();
            _Initialize();
            _ownClient = true;
        }

        /// <summary>
        /// Конструктор с заранее созданным клиентом.
        /// </summary>
        /// <param name="client"></param>
        public IrbisScript
            (
                ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            Client = client;
            _Initialize();
        }

        #endregion

        #region Private members

        private readonly bool _ownClient;
        private static bool _typesRegistered;

        /// <summary>
        /// Внутренняя инициализация.
        /// </summary>
        private void _Initialize()
        {
            RegisterIrbisTypes();
            Engine = new Script(CoreModules.Preset_Complete);

            SetGlobal ("Client", Client);

            foreach (Type type in UserData.GetRegisteredTypes())
            {
                if ((type.Namespace != null)
                    && type.Namespace.StartsWith("ManagedClient"))
                {
                    SetGlobal
                        (
                            type.Name,
                            type
                        );
                }
            }


            SetRecord(null);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Вызов Lua-функции и получение результата.
        /// </summary>
        public DynValue CallFunction
            (
                string name,
                params object[] args
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            DynValue function = Engine.Globals.Get(name);
            if (function.Type != DataType.Function)
            {
                throw new ArgumentOutOfRangeException("name");
            }

            DynValue result = Engine.Call
                (
                    function,
                    args
                );

            return result;
        }

        /// <summary>
        /// Исполнение Lua-скрипта из файла
        /// и получение результата.
        /// </summary>
        public DynValue DoFile
            (
                string filename
            )
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            DynValue result = Engine.DoFile
                (
                    filename
                );

            return result;
        }

        /// <summary>
        /// Исполнение Lua-кода и получение результата.
        /// </summary>
        public DynValue DoString
            (
                string code
            )
        {
            if (string.IsNullOrEmpty(code))
            {
                return DynValue.Nil;
            }
            return Engine.DoString(code);
        }

        /// <summary>
        /// Регистрация типов, помеченных в данной сборке атрибутом
        /// <see cref="MoonSharpUserDataAttribute"/>
        /// </summary>
        public static void RegisterIrbisTypes()
        {
            if (!_typesRegistered)
            {
                UserData.RegisterAssembly(typeof (IrbisScript).Assembly);
                UserData.RegisterType<Version>();
                _typesRegistered = true;
            }
        }


        /// <summary>
        /// Установка глобального значения.
        /// </summary>
        public IrbisScript SetGlobal
            (
                string name,
                object value
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            Engine.Globals.Set
                (
                    name,
                    DynValue.FromObject
                    (
                        Engine,
                        value
                    )
                );
            return this;
        }

        /// <summary>
        /// Установка новой текущей записи.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public IrbisScript SetRecord
            (
                IrbisRecord record
            )
        {
            Record = record;
            SetGlobal("Record", record);
            return this;
        }

        #endregion

        #region IDisposable methods

        public void Dispose()
        {
            if (Client != null)
            {
                if (_ownClient)
                {
                    Client.Disconnect();
                    Client = null;
                }
            }

        }

        #endregion
    }
}
