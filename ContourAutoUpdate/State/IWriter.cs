using System;
using System.Collections.Generic;

namespace ContourAutoUpdate.State
{
    internal interface IWriter : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">Если null, значение будет удалено.</param>
        void Write(string name, string value);

        /// <summary>
        /// Записывает какое то значение. При повторном вызове записывает то-же самое значение.
        /// <para/>Чтобы восстановить, надо вызвать <see cref="LinkRef{T}(string, T)"/> и <see cref="ReadRef(string)"/>.
        /// </summary>
        void WriteRef<T>(string name, T obj) where T : class;

        /// <summary>
        /// Записывает объект и предназначает для его ссылку (<see cref="WriteRef"/>). При повторном вызове записывает только ссылку.
        /// <para/>Восстановка с помощью <see cref="Restore"/>.
        /// </summary>
        void Write<T>(string name, T obj) where T : ISaveable;

        /// <summary>
        /// Создаёт объект, который будет вставлять значения глубже, в указанную секцию.
        /// <para/>Сама секция не должна обязательно создаться. Она может создаться при вставки первого значения.
        /// </summary>
        IWriter Section(string sectionName);
        /// <summary>
        /// Должно переименовать секцию. Существующую секцию с новым именем должно удалить.
        /// </summary>
        void RenameSection(string sectionName, string newName);

        /// <summary>Должно вернуть null, если не находит значения.</summary>
        string Read(string name);
        /// <summary>
        /// Читает значение и её связывает с инстанцией указанного объекта. Потом при чтении такого
        /// значение через <see cref="ReadRef(string)"/>, функция вернёт этот объект.
        /// </summary>
        void LinkRef<T>(string refKey, T obj) where T : class;
        /// <summary>Должно вернуть объект, который был зарегистрирован через <see cref="LinkRef{T}(string, T)"/>, или null если такое поле не существует.</summary>
        object ReadRef(string name);

        /// <summary>
        /// Восстановка объекта, сохранённого с <see cref="Write{T}(string, T)"/>.
        /// </summary>
        T Read<T>(string name, Func<T> createFunc) where T : ISaveable;

        /// <summary>Список наименований значений и секций.</summary>
        IEnumerable<string> GetNames();
    }
}
