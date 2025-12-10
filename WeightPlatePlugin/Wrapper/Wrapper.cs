using System;
using Kompas6API5;
using Kompas6Constants3D;

namespace WeightPlatePlugin.Wrapper
{
    /// <summary>
    /// Обёртка над Kompas 3D API.
    /// Отвечает за подключение к КОМПАСу и низкоуровневые операции построения.
    /// </summary>
    public class Wrapper
    {
        /// <summary>
        /// Главный объект КОМПАС-3D.
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// 3D-документ.
        /// </summary>
        private ksDocument3D _doc3D;

        /// <summary>
        /// Верхняя деталь (Part_Type.pTop_Part).
        /// </summary>
        private ksPart _topPart;

        /// <summary>
        /// Текущий 2D-документ эскиза.
        /// </summary>
        private ksDocument2D _current2dDoc;

        /// <summary>
        /// Подключается к запущенному КОМПАС-3D или запускает новый процесс.
        /// </summary>
        public void AttachOrRunCAD()
        {
            if (_kompas != null)
            {
                return;
            }

            //TODO: RSDN +
            var t = Type.GetTypeFromProgID("KOMPAS.Application.5");
            if (t == null)
            {
                throw new InvalidOperationException(
                    "Не найден ProgID KOMPAS.Application.5");
            }

            //TODO: RSDN +
            _kompas = (KompasObject)Activator.CreateInstance(t)
                      ?? throw new InvalidOperationException(
                          "Не удалось создать KompasObject.");

            _kompas.Visible = true;
            _kompas.ActivateControllerAPI();
        }

        /// <summary>
        /// Создаёт новый 3D-документ (деталь) и получает верхнюю деталь.
        /// </summary>
        public void CreateDocument3D()
        {
            if (_kompas == null)
            {
                //TODO: RSDN +
                throw new InvalidOperationException(
                    "Kompas не инициализирован. Сначала вызови AttachOrRunCAD().");
            }

            //TODO: RSDN +
            _doc3D = (ksDocument3D)_kompas.Document3D()
                     ?? throw new InvalidOperationException(
                         "Не удалось получить ksDocument3D.");

            _doc3D.Create();

            _topPart = (ksPart)_doc3D.GetPart((short)Part_Type.pTop_Part)
                       ?? throw new InvalidOperationException(
                           "Не удалось получить верхнюю деталь.");
        }

        /// <summary>
        /// Возвращает верхнюю деталь (на случай, если захотим использовать её напрямую).
        /// </summary>
        public object GetTopPart()
        {
            if (_topPart == null)
            {
                throw new InvalidOperationException(
                    "Документ ещё не создан. Вызови CreateDocument3D().");
            }

            return _topPart;
        }

        /// <summary>
        /// Создаёт эскиз на базовой плоскости ("XOY", "XOZ", "YOZ").
        /// Возвращает объект эскиза (ksEntity).
        /// </summary>
        public object CreateSketchOnPlane(string plane)
        {
            if (_topPart == null)
            {
                //TODO: RSDN +
                throw new InvalidOperationException(
                    "Часть не инициализирована. Вызови CreateDocument3D().");
            }

            short planeType = plane?.ToUpperInvariant() switch
            {
                "XOY" or "XY" => (short)Obj3dType.o3d_planeXOY,
                "XOZ" or "XZ" => (short)Obj3dType.o3d_planeXOZ,
                "YOZ" or "YZ" => (short)Obj3dType.o3d_planeYOZ,
                _ => (short)Obj3dType.o3d_planeXOY
            };

            //TODO: RSDN +
            var basePlane = (ksEntity)_topPart.GetDefaultEntity(planeType)
                           ?? throw new InvalidOperationException(
                               "Не удалось получить базовую плоскость.");

            //TODO: RSDN +
            var sketchEntity = (ksEntity)_topPart
                                .NewEntity((short)Obj3dType.o3d_sketch)
                               ?? throw new InvalidOperationException(
                                   "Не удалось создать сущность o3d_sketch.");

            var sketchDef = (ksSketchDefinition)sketchEntity.GetDefinition();
            sketchDef.SetPlane(basePlane);
            sketchEntity.Create();

            _current2dDoc = (ksDocument2D)sketchDef.BeginEdit();

            return sketchEntity;
        }

        /// <summary>
        /// Рисует окружность на активном эскизе.
        /// </summary>
        public void DrawCircle(double xc, double yc, double radius)
        {
            if (_current2dDoc == null)
            {
                //TODO: RSDN +
                throw new InvalidOperationException("Нет активного 2D-эскиза." +
                    " Сначала вызови CreateSketchOnPlane().");
            }

            _current2dDoc.ksCircle(xc, yc, radius, 1);
        }

        /// <summary>
        /// Завершает редактирование эскиза.
        /// </summary>
        public void FinishSketch(object sketch)
        {
            if (sketch is not ksEntity sketchEntity)
            {

                //TODO: RSDN +
                throw new ArgumentException("Ожидался объект эскиза (ksEntity).", 
                    nameof(sketch));
            }

            var sketchDef = (ksSketchDefinition)sketchEntity.GetDefinition();
            sketchDef.EndEdit();
            _current2dDoc = null;
        }

        /// <summary>
        /// Создаёт тело вращения по профилю.
        /// Реализовано как выдавливание окружности на толщину (эквивалент цилиндру D × T).
        /// </summary>
        /// <param name="sketch">Эскиз окружности.</param>
        /// <param name="axis">Ось вращения (пока не используется).</param>
        /// <param name="thickness">Толщина диска T (глубина выдавливания).</param>
        public void BossByRevolve(object sketch, string axis, double thickness)
        {
            //TODO: RSDN +
            if (_topPart == null)
            {
                //TODO: refactor +
                throw new InvalidOperationException(
                                    "Часть не инициализирована. Вызови CreateDocument3D().");
            }   

            //TODO: RSDN +
            if (sketch is not ksEntity sketchEntity)
            {
                //TODO: refactor +
                throw new ArgumentException(
                    "Ожидался объект эскиза (ksEntity).", nameof(sketch));
            }
                
                

            var bossEntity =
                (ksEntity)_topPart.NewEntity((short)Obj3dType.o3d_bossExtrusion)
                ?? throw new InvalidOperationException(
                    "Не удалось создать сущность o3d_bossExtrusion.");

            var bossDef = (ksBossExtrusionDefinition)bossEntity.GetDefinition();
            var extParam = (ksExtrusionParam)bossDef.ExtrusionParam();

            bossDef.SetSketch(sketchEntity);

            extParam.direction = (short)Direction_Type.dtNormal;
            extParam.typeNormal = (short)End_Type.etBlind;
            extParam.depthNormal = thickness;   // весь диск в одну сторону от XOY

            bossEntity.Create();
        }



        /// <summary>
        /// "Сквозной" вырез по эскизу через всю деталь.
        /// Реализован как вырез на глубину, немного большую толщины диска.
        /// </summary>
        /// <param name="sketch">Эскиз отверстия.</param>
        /// <param name="bodyThickness">Толщина тела (диска) в мм.</param>
        /// <param name="forward">Направление относительно нормали эскиза.</param>
        public void CutByExtrusionThroughAll(object sketch, double bodyThickness, bool forward)
        {

            if (bodyThickness <= 0)
            {
                //TODO: refactor +
                //TODO: RSDN +
                throw new ArgumentOutOfRangeException(nameof(bodyThickness),
                    "Толщина тела должна быть > 0.");
            }
                

            // делаем небольшой запас, чтобы гарантированно пройти насквозь
            var depth = bodyThickness * 1.2;

            CutByExtrusionDepth(sketch, depth, forward);
        }


        /// <summary>
        /// Вырез по эскизу до глубины (углубление G).
        /// </summary>
        public void CutByExtrusionDepth(object sketch, double depth, bool forward)
        {
            if (_topPart == null)
            {
                throw new InvalidOperationException(
                    "Часть не инициализирована. Вызови CreateDocument3D().");
            }
                

            if (sketch is not ksEntity sketchEntity)
            {
                throw new ArgumentException("Ожидался объект эскиза (ksEntity).",
                    nameof(sketch));
            }
                

            var cutEntity =
                (ksEntity)_topPart.NewEntity((short)Obj3dType.o3d_cutExtrusion)
                ?? throw new InvalidOperationException(
                    "Не удалось создать сущность o3d_cutExtrusion.");

            var cutDef = (ksCutExtrusionDefinition)cutEntity.GetDefinition();

            // это именно вырезание, а не добавление материала
            cutDef.cut = true;

            // направление выдавливания
            cutDef.directionType = (short)(
                forward
                    ? Direction_Type.dtNormal   // от плоскости вперёд
                    : Direction_Type.dtReverse  // от плоскости назад
            );

            // side1: при dtNormal = true, при dtReverse = false
            bool side1 = forward;

            cutDef.SetSideParam(
                side1,
                (short)End_Type.etBlind,
                Math.Abs(depth),
                0.0,
                false);

            cutDef.SetSketch(sketchEntity);

            cutEntity.Create();
        }



        /// <summary>
        /// Применение скруглений по радиусу radius.
        /// Если edges == null, скругляет все рёбра детали.
        /// Если edges — ksEntityCollection, скругляет только указанные рёбра.
        /// </summary>
        public void ApplyChamferOrFillet(double radius, object? edges)
        {
            if (_topPart == null)
            {
                throw new InvalidOperationException(
                   "Часть не инициализирована. Вызови CreateDocument3D().");
            }
            

            if (radius <= 0)
            {
                return;
            }
                

            // Создаём операцию скругления
            var filletEntity =
                (ksEntity)_topPart.NewEntity((short)Obj3dType.o3d_fillet)
                ?? throw new InvalidOperationException(
                    "Не удалось создать сущность o3d_fillet.");

            var filletDef = (ksFilletDefinition)filletEntity.GetDefinition();

            // Радиус скругления
            filletDef.radius = radius;

            // Коллекция рёбер для скругления
            var filletEdges = (ksEntityCollection)filletDef.array();
            filletEdges.Clear();

            if (edges is ksEntityCollection customEdges)
            {
                // Пользовательская коллекция рёбер
                int count = customEdges.GetCount();
                for (int i = 0; i < count; i++)
                {
                    var edge = (ksEntity)customEdges.GetByIndex(i);
                    if (edge != null)
                    {
                        filletEdges.Add(edge);
                    }
                }
            }
            else
            {
                // Скругляем все рёбра детали
                var allEdges =
                    (ksEntityCollection)_topPart.EntityCollection((short)Obj3dType.o3d_edge);

                int count = allEdges.GetCount();
                for (int i = 0; i < count; i++)
                {
                    var edge = (ksEntity)allEdges.GetByIndex(i);
                    if (edge != null)
                    {
                        filletEdges.Add(edge);
                    }
                }
            }

            // Создаём операцию
            filletEntity.Create();
        }


        /// <summary>
        /// Сохранение модели на диск.
        /// </summary>
        public void SaveAs(string path)
        {
            if (_doc3D == null)
            {
                throw new InvalidOperationException(
                    "Документ не создан. Вызови CreateDocument3D().");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(
                    "Путь к файлу не задан.", nameof(path));
            }

            _doc3D.SaveAs(path);
        }

        /// <summary>
        /// Создаёт эскиз на плоскости, смещённой от XOY на offset по нормали.
        /// </summary>
        public object CreateSketchOnOffsetPlaneFromXOY(double offset)
        {
            if (_topPart == null)
                throw new InvalidOperationException(
                    "Часть не инициализирована. Вызови CreateDocument3D().");

            var basePlane = (ksEntity)_topPart.GetDefaultEntity((short)Obj3dType.o3d_planeXOY)
                           ?? throw new InvalidOperationException(
                               "Не удалось получить базовую плоскость XOY.");

            var offsetPlaneEntity =
                (ksEntity)_topPart.NewEntity((short)Obj3dType.o3d_planeOffset)
                ?? throw new InvalidOperationException(
                    "Не удалось создать сущность o3d_planeOffset.");

            var offsetDef = (ksPlaneOffsetDefinition)offsetPlaneEntity.GetDefinition();
            offsetDef.SetPlane(basePlane);
            offsetDef.direction = true;     // смещение вперёд по нормали XOY
            offsetDef.offset = offset;

            offsetPlaneEntity.Create();

            var sketchEntity =
                (ksEntity)_topPart.NewEntity((short)Obj3dType.o3d_sketch)
                ?? throw new InvalidOperationException(
                    "Не удалось создать сущность o3d_sketch.");

            var sketchDef = (ksSketchDefinition)sketchEntity.GetDefinition();
            sketchDef.SetPlane(offsetPlaneEntity);
            sketchEntity.Create();

            _current2dDoc = (ksDocument2D)sketchDef.BeginEdit();

            return sketchEntity;
        }


    }
}
