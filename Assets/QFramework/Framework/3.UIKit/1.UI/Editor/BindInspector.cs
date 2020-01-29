using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QFramework
{
    [CustomEditor(typeof(Bind), true)]
    public class BindInspector : UnityEditor.Editor
    {
        class LocaleText
        {
            public static string MarkType
            {
                get { return Language.IsChinese ? " 标记类型:" : " Mark Type:"; }
            }

            public static string Type
            {
                get { return Language.IsChinese ? " 类型:" : " Type:"; }
            }

            public static string ClassName
            {
                get { return Language.IsChinese ? " 生成类名:" : " Generate Class Name:"; }
            }

            public static string Comment
            {
                get { return Language.IsChinese ? " 注释" : " Comment"; }
            }

            public static string BelongsTo
            {
                get { return Language.IsChinese ? " 属于:" : " Belongs 2:"; }
            }

            public static string Select
            {
                get { return Language.IsChinese ? "选择" : "Select"; }
            }

            public static string Generate
            {
                get { return Language.IsChinese ? " 生成代码" : " Generate Code"; }
            }
        }


        private Bind mBindScript
        {
            get { return target as Bind; }
        }

        private VerticalLayout   mRootLayout;
        private HorizontalLayout mComponentLine;
        private HorizontalLayout mClassnameLine;

        private void OnEnable()
        {
            mRootLayout = new VerticalLayout("box");

            new SpaceView()
                .AddTo(mRootLayout);

            var markTypeLine = new HorizontalLayout()
                .AddTo(mRootLayout);

            new LabelView(LocaleText.MarkType)
                .FontSize(12)
                .Width(60)
                .AddTo(markTypeLine);

            var enumPopupView = new EnumPopupView(mBindScript.MarkType)
                .AddTo(markTypeLine);

            enumPopupView.ValueProperty.Bind(newValue =>
            {
                mBindScript.MarkType = (BindType) newValue;

                OnRefresh();
            });


            new SpaceView()
                .AddTo(mRootLayout);

            new CustomView(() =>
            {
                if (mBindScript.CustomComponentName == null ||
                    string.IsNullOrEmpty(mBindScript.CustomComponentName.Trim()))
                {
                    mBindScript.CustomComponentName = mBindScript.name;
                }
            }).AddTo(mRootLayout);


            mComponentLine = new HorizontalLayout();

            new LabelView(LocaleText.Type)
                .Width(60)
                .FontSize(12)
                .AddTo(mComponentLine);

            if (mBindScript.MarkType == BindType.DefaultUnityElement)
            {

                var components = mBindScript.GetComponents<Component>();

                var componentNames = components.Where(c => c.GetType() != typeof(Bind))
                    .Select(c => c.GetType().FullName)
                    .ToArray();

                var componentNameIndex = 0;

                componentNameIndex = componentNames.ToList()
                    .FindIndex((componentName) => componentName.Contains(mBindScript.ComponentName));

                if (componentNameIndex == -1 || componentNameIndex >= componentNames.Length)
                {
                    componentNameIndex = 0;
                }
                
                mBindScript.ComponentName = componentNames[componentNameIndex];

                new PopupView(componentNameIndex, componentNames)
                    .AddTo(mComponentLine)
                    .IndexProperty.Bind((index) => { mBindScript.ComponentName = componentNames[index]; });
            }

            mComponentLine.AddTo(mRootLayout);
            

            new SpaceView()
                .AddTo(mRootLayout);

            var belongsTo = new HorizontalLayout()
                .AddTo(mRootLayout);

            new LabelView(LocaleText.BelongsTo)
                .Width(60)
                .FontSize(12)
                .AddTo(belongsTo);

            new LabelView(CodeGenUtil.GetBindBelongs2(target as Bind))
                .Width(200)
                .FontSize(12)
                .AddTo(belongsTo);


            new ButtonView(LocaleText.Select, () =>
                {
                    Selection.objects = new[]
                    {
                        CodeGenUtil.GetBindBelongs2GameObject(target as Bind)
                    };
                })
                .Width(60)
                .AddTo(belongsTo);

            mClassnameLine = new HorizontalLayout();

            new LabelView(LocaleText.ClassName)
                .Width(60)
                .FontSize(12)
                .AddTo(mClassnameLine);

            new TextView(mBindScript.CustomComponentName)
                .AddTo(mClassnameLine)
                .Content.Bind(newValue => { mBindScript.CustomComponentName = newValue; });

            mClassnameLine.AddTo(mRootLayout);

            new SpaceView()
                .AddTo(mRootLayout);

            new LabelView(LocaleText.Comment)
                .FontSize(12)
                .AddTo(mRootLayout);

            new SpaceView()
                .AddTo(mRootLayout);

            new TextAreaView(mBindScript.Comment)
                .Height(100)
                .AddTo(mRootLayout)
                .Content.Bind(newValue => mBindScript.CustomComment = newValue);

            var bind = target as Bind;
            var rootGameObj = CodeGenUtil.GetBindBelongs2GameObject(bind);


            if (rootGameObj.transform.GetComponent("ILKitBehaviour")) 
            {
                
            }
            else if (rootGameObj.transform.IsUIPanel())
            {
                new ButtonView(LocaleText.Generate + " " + CodeGenUtil.GetBindBelongs2(bind),
                        () =>
                        {
                            var rootPrefabObj = PrefabUtility.GetCorrespondingObjectFromSource(rootGameObj);


                            UICodeGenerator.DoCreateCode(new[] {rootPrefabObj});
                        })
                    .Height(30)
                    .AddTo(mRootLayout);
            }
            else if (rootGameObj.transform.IsViewController())
            {
                new ButtonView(LocaleText.Generate + " " + CodeGenUtil.GetBindBelongs2(bind),
                        () =>
                        {
                            CreateViewControllerCode.DoCreateCodeFromScene(bind.gameObject);
                        })
                    .Height(30)
                    .AddTo(mRootLayout);
            }


            OnRefresh();
        }

        private void OnRefresh()
        {
            if (mBindScript.MarkType == BindType.DefaultUnityElement)
            {
                mComponentLine.Show();
                mClassnameLine.Hide();
            }
            else
            {
                mClassnameLine.Show();
                mComponentLine.Hide();
            }
        }

        private void OnDisable()
        {
            mRootLayout.Clear();
            mRootLayout = null;
        }

        public override void OnInspectorGUI()
        {
            mRootLayout.DrawGUI();
            base.OnInspectorGUI();
        }
    }
}