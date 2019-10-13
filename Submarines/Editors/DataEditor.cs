﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine.DataPlus;
using Engine.EventSystem;
using Engine.Helpers;
using Engine.Visualization;
using Engine.Visualization.Scroll;

namespace Submarines.Editors
{
    /// <summary>
    /// see EngineTools.DataEditor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataEditor<T> : FilteredScrollViewWindow where T : class
    {
        private Action<T> _update;
        private Action<T> _cancel;
        private T _objectToEdit;

        private Dictionary<string, Type> _scrollsCreators = null;
        private Dictionary<string, Action<ScrollItem>> _scrollActions = null;

        public void InitWindow(ViewManager viewManager, T objectToEdit, Action<T> update, Action<T> cancel = null) {
            _update = update;
            _cancel = cancel;
            _objectToEdit = objectToEdit;

            InitWindow("Редактор для " + objectToEdit.GetType(), viewManager, showOkButton: true, showNewButton: false);
        }

        public DataEditor<T> AddEditor(string name, Type type, Action<ScrollItem> action) {
            if (_scrollsCreators == null)
                _scrollsCreators = new Dictionary<string, Type>();

            _scrollsCreators.Add(name, type);

            if (action != null) {
                if (_scrollActions == null)
                    _scrollActions = new Dictionary<string, Action<ScrollItem>>();

                Action<ScrollItem> value;
                if (!_scrollActions.TryGetValue(name, out value))
                    _scrollActions.Add(name, null);

                _scrollActions[name] += action;
            }

            return this;
        }

        protected override void InitScrollItems() {
            var row = 0;
            var t = _objectToEdit.GetType();
            var mi = t.GetMembers().Where(m => m.MemberType == MemberTypes.Property);
            foreach (PropertyInfo item in mi) {
                if (AttributesHelper.IsHasAttribute<SkipEditEditorAttribute>(item))
                    continue;

                if (_scrollsCreators != null &&
                    AttributesHelper.IsHasAttribute<MemberSpecialEditorAttribute>(item)) {
                    var type = AttributesHelper.GetAttribute<MemberSpecialEditorAttribute>(item);
                    foreach (var scrollCreator in _scrollsCreators) {
                        if (type.EditorType != scrollCreator.Key)
                            continue;

                        var scrollItem = (MemberBaseScrollView<T>)ReflectionHelper.CreateGenericType(scrollCreator.Value, typeof(T));
                        ViewScroll.AddComponent(scrollItem);
                        // вызывается до инициализации объекта. но возможно нужно будет и потом ещё инициализировать
                        if (_scrollActions != null && _scrollActions.ContainsKey(scrollCreator.Key))
                            _scrollActions[scrollCreator.Key](scrollItem);
                        scrollItem.InitValueEditor(_objectToEdit, item);
                        scrollItem.SetParams(10, (row) * 60 + 10, 950, 50, "item" + item);
                    }
                } else
                if (item.PropertyType.IsEnum) {
                    var scrollItem = new MemberEnumScrollView<T>();
                    ViewScroll.AddComponent(scrollItem);
                    scrollItem.InitValueEditor(_objectToEdit, item);
                    scrollItem.SetParams(10, (row) * 60 + 10, 950, 50, "enum" + item.Name);
                } else {
                    var scrollItem = new MemberScrollView<T>();
                    ViewScroll.AddComponent(scrollItem);
                    scrollItem.InitValueEditor(_objectToEdit, item);
                    scrollItem.SetParams(10, (row) * 60 + 10, 950, 50, "item" + item);
                    if (item.PropertyType.Name == "String")
                        scrollItem.SetupMemberEditor(getValue: str => str);
                }

                row++;
            }
        }

        protected override void InitButtonOk(ViewButton btnOk) {
            base.InitButtonOk(btnOk);
            btnOk.Caption = "Ok";
            btnOk.Hint = "Сохранить";
        }

        protected override void OkCommand() {
            GetValues();
            if (_update == null)
                return;
            var updater = _update;
            Checkers.AddToCheckOnce(() => updater?.Invoke(_objectToEdit));
        }

        /// <summary>
        /// Получить значения переменных
        /// </summary>
        private void GetValues() {
            var a = ViewScroll.GetItems();
            foreach (var item in a) {
                var m = item as MemberBaseScrollView<T>;
                if (m == null)
                    continue;

                m.SetValue(_objectToEdit);
            }
        }

        protected override void CloseWindow() {
            base.CloseWindow();
            _update = null;
            _cancel = null;
        }

        protected override void CancelCommand() {
            if (_cancel == null)
                return;
            var canceler = _cancel;
            Checkers.AddToCheckOnce(() => canceler?.Invoke(_objectToEdit));
        }

        public override void DrawObject(VisualizationProvider visualizationProvider) {
            visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
            visualizationProvider.Box(X, Y, Width, Height);
            base.DrawObject(visualizationProvider);
        }
    }
}