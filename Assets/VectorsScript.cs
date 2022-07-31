using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class VectorsScript : MonoBehaviour
{
    [Header("Distance")]
    public List<GameObject> distanceVectors = new List<GameObject>(2);

    [Header("Magnitude")]
    public List<GameObject> magnitudeVector = new List<GameObject>(1);
    public bool isMagnitudeDrawVector = false;

    [Header("SqrMagnitude")]
    public List<GameObject> sqrMagnitudeVector = new List<GameObject>(1);
    public bool isSqrMagnitudeDrawVector = false;

    [Header("Angle")]
    public List<GameObject> angleVectors = new List<GameObject>(2);
    public bool isAngleVectorsDrawVectors = false;

    [Header("ClampMagnitude")]
    public GameObject cmObject;
    public bool isClampingDrawVector = false;
    [Range(1,10)]
    public float cmObjectLength = 1;

    [Header("Cross")]
    public List<GameObject> crossVectors = new List<GameObject>(3);
    public bool isCrossDrawVector = false;

    [Header("Dot")]
    public List<GameObject> dotVectors = new List<GameObject>(2);
    public bool isDotDrawVectors = false;

    [Header("Lerp")]
    public List<GameObject> lerpVectors = new List<GameObject>(2);
    public bool isLerpDrawVectors = false;
    [Range(0, 1)]
    public float lerpInterpolant = 0;

    [Header("MoveTowards")]
    public List<GameObject> moveTowardsVectors = new List<GameObject>(2);
    public bool isMoveTowardsDrawVectors = false;
    [Range(0, 5)]
    public float maxDistanceDelta = 0;

    [Header("Project")]
    public List<GameObject> projectVectors = new List<GameObject>(2);
    public bool isProjectDrawVectors = false;

    [Header("Reflect")]
    public List<GameObject> reflectVectors = new List<GameObject>(2);
    public bool isReflectDrawVectors = false;

    [Header("RotateTowards")]
    public List<GameObject> rotateTowardsVectors = new List<GameObject>(2);
    /// <summary>Максимальный угол в радианах, разрешенный для этого поворота.</summary>
    [Range(0, 5)]
    public float maxRadiansDelta = 0.5f;
    /// <summary>Максимально допустимое изменение величины вектора для этого поворота.</summary>
    [Range(0, 5)]
    public float maxMagnitudeDelta = 0.5f;

    [Header("Scale")]
    public List<GameObject> scaleVectors = new List<GameObject>(2);
    public bool isScaleDrawVectors = false;

    [Header("SignedAngle")]
    public List<GameObject> signedAngleVectors = new List<GameObject>(2);
    public bool isSignedAngleDrawVectors = false;

    [Header("Slerp")]
    public List<GameObject> slerpVectors = new List<GameObject>(2);
    public bool isSlerpVectorsDrawVectors = false;
    [Range(0, 1)]
    public float sLerpInterpolant = 0.5f;

    private void OnDrawGizmos()
    {
        // Distance - расстояние между двумя векторами (float)
        if(distanceVectors.Count == 2)
        {
            Transform firstVectorTranmsform = distanceVectors[0].transform;
            Transform secondVectorTranmsform = distanceVectors[1].transform;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(firstVectorTranmsform.position, secondVectorTranmsform.position);

            var text = distanceVectors[0].GetComponent<TextMeshPro>();

            // Первый вариант
            //text.text = $"distance {Vector3.Distance(firstVectorTranmsform.position, secondVectorTranmsform.position)}";

            // Второй вариант
            text.text = $"distance: {(firstVectorTranmsform.position - secondVectorTranmsform.position).magnitude}";

            // Разницы в производиельности почти нет
        }

        // Magnitude - длина (модуль) вектора (float)
        if (magnitudeVector.Count == 1)
        {
            if (isMagnitudeDrawVector) 
                Gizmos.DrawLine(Vector3.zero, magnitudeVector[0].transform.position);

            var text = magnitudeVector[0].GetComponent<TextMeshPro>();
            text.text = $"magnitude {magnitudeVector[0].transform.position.magnitude}";
        }

        // SqrMagnitude - сумма квадратов катетов
        if (sqrMagnitudeVector.Count == 1)
        {
            if (isSqrMagnitudeDrawVector)
                Gizmos.DrawLine(Vector3.zero, sqrMagnitudeVector[0].transform.position);

            var text = sqrMagnitudeVector[0].GetComponent<TextMeshPro>();
            text.text = $"sqrMagnitude {sqrMagnitudeVector[0].transform.position.sqrMagnitude}";
        }

        // Angle - угол в радиусах между двумя векторами
        // - возвращаемый угол - угол без знака между двумя векторами.
        // - используется меньший из двух возможных углов между двумя векторами.
        // - результат никогда не превышает 180 градусов.
        if (angleVectors.Count == 2)
        {
            var sourcePosition = angleVectors[0].transform.position;
            var targetPosition = angleVectors[1].transform.position;
            Vector3 targetDir = targetPosition - sourcePosition;

            if (isAngleVectorsDrawVectors)
            {
                Gizmos.DrawLine(Vector3.zero, sourcePosition);
                Gizmos.DrawLine(Vector3.zero, targetPosition);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(Vector3.zero, targetDir);
            }

            float angle = Vector3.Angle(targetDir, angleVectors[0].transform.forward);

            var text = angleVectors[0].GetComponent<TextMeshPro>();
            text.text = $"angle: {angle}";
        }

        // ClampMagnitude
        // - Возвращает копию vector с величиной, ограниченной значением maxLength.
        if (cmObject != null)
        {
            cmObject.transform.position = Vector3.ClampMagnitude(cmObject.transform.position, cmObjectLength);

            if (isClampingDrawVector)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(Vector3.zero, cmObject.transform.position);
            }

            var text = cmObject.GetComponent<TextMeshPro>();
            text.text = $"clamping: {cmObjectLength}";

        }

        // Cross
        // - перекрестное произведение двух векторов
        // - в результате перекрестного произведения двух векторов получается третий вектор,
        // который перпендикулярен двум входным векторам
        // - величина результата равна величине двух входных данных, умноженных вместе,
        // а затем умноженных на синус угла между входными данными
        if (crossVectors.Count == 3)
        {
            Gizmos.color = Color.green;
 
            Vector3 a = crossVectors[0].transform.position;
            Vector3 b = crossVectors[1].transform.position;
            Vector3 c = crossVectors[2].transform.position;

            if (isCrossDrawVector)
            {
                Gizmos.DrawLine(Vector3.zero, a);
                Gizmos.DrawLine(Vector3.zero, b);
                Gizmos.DrawLine(Vector3.zero, c);
            }

            // Находим векторы, соответствующие двум сторонам треугольника.
            Vector3 side1 = Vector3.ClampMagnitude(b - a, 1);
            Vector3 side2 = Vector3.ClampMagnitude(c - a, 1);

            // Пересекаем векторы, чтобы получить перпендикулярный вектор, затем нормализуем его.
            // - внутри ограничиваем длину вектора
            Vector3 cross = Vector3.Cross(side1, side2).normalized;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(b, cross);

            var text = crossVectors[0].GetComponent<TextMeshPro>();
            text.text = $"cross\nvector";

        }

        // Dot - cкалярное произведение двух векторов
        // - это число с плавающей запятой, равное произведению величин двух векторов,
        // а затем умножению на косинус угла между ними.
        // - для нормализованных векторов точка возвращает 1, если они указывают в одном и том же направлении,
        //  -1, если они указывают в совершенно противоположных направлениях, и ноль, если векторы перпендикулярны.
        if (dotVectors.Count == 2)
        {
            Gizmos.color = Color.green;

            if (isDotDrawVectors)
            {
                Gizmos.DrawLine(Vector3.zero, dotVectors[0].transform.position);
                Gizmos.DrawLine(Vector3.zero, dotVectors[1].transform.position);
            }

            Vector3 forward = dotVectors[0].transform.TransformDirection(dotVectors[0].transform.forward);
            Vector3 toOther = dotVectors[1].transform.position - dotVectors[0].transform.position;

            var text = dotVectors[0].GetComponent<TextMeshPro>();
            text.text = $"dot: {Vector3.Dot(forward, toOther)}";
        }

        // Lerp - Линейная интерполяция между двумя точками.
        // - интерполирует между точками a и b интерполянтом t.
        // - интерполянт t фиксируется в диапазоне[0, 1].
        // - чаще всего используется для нахождения точки на некотором участке пути вдоль линии между двумя конечными точками(например, для постепенного перемещения объекта между этими точками).
        if (lerpVectors.Count == 2)
        {
            var a = lerpVectors[0].transform.position;
            var b = lerpVectors[1].transform.position;


            if (isLerpDrawVectors)
            {
                Gizmos.DrawLine(Vector3.zero, a);
                Gizmos.DrawLine(Vector3.zero, b);
            }

            Vector3 interPolate = Vector3.Lerp(a, b, lerpInterpolant);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, interPolate);
            Gizmos.DrawSphere(interPolate, 0.1f);
        }

        // Max - нахожедние максимального вектора из двух (без примера)
        // Min - нахожедние минимального вектора из двух (без примера)

        // MoveTowards
        // - вычисляет положение между точками, указанными как current(a) и target(b), перемещаясь не дальше, чем расстояние, указанное как maxDistanceDelta.
        if (moveTowardsVectors.Count == 2)
        {
            Vector3 a = moveTowardsVectors[0].transform.position;
            Vector3 b = moveTowardsVectors[1].transform.position;

            if (isMoveTowardsDrawVectors)
            {
                Gizmos.DrawLine(Vector3.zero, a);
                Gizmos.DrawLine(Vector3.zero, b);
            }

            Vector3 moveTowards = Vector3.MoveTowards(a, b, maxDistanceDelta);
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(a, moveTowards);
            Gizmos.DrawCube(moveTowards, new Vector3(0.2f, 0.2f, 0.2f));
        }

        // Vector3.Normalize - нормализует вектор (его длина становится = 1), изменяет исходный вектор
        // - для сохранения исходного вектора используется его свойство normalized
        // - если вектор мал для нормализации, он приравнивается к нулевому
        // - без примера

        // OrthoNormalize
        // - делает векторы нормализованными и ортогональными (расположенными под углом 90 градусов) друг другу

        // Project
        // - проецирует вектор на другой вектор
        // - результатом проекции одного вектора на другой также является вектор (имеет направление и длину).
        if (projectVectors.Count == 2)
        {
            Vector3 a = projectVectors[0].transform.position;
            Vector3 b = projectVectors[1].transform.position;

            Gizmos.color = Color.green;

            if (isProjectDrawVectors)
            {
                Gizmos.DrawLine(Vector3.zero, a);
                Gizmos.DrawLine(Vector3.zero, b);
            }

            Vector3 projectVector = Vector3.Project(a, b);
            Gizmos.color = Color.red;

            Gizmos.DrawLine(Vector3.zero, projectVector);
            Gizmos.DrawSphere(projectVector, 0.1f);
        }

        // ProjectOnPlane
        // - проецирует вектор на плоскость, заданную нормалью ортогональной плоскости.
        // - без примера

        // Reflect
        if (reflectVectors.Count == 2)
        {
            Transform inDirection = reflectVectors[0].transform;
            Transform source = reflectVectors[1].transform;


            if (isReflectDrawVectors)
            {
                Gizmos.color = Color.green;
                Debug.DrawRay(Vector3.zero, inDirection.transform.position, Color.green);
                Debug.DrawRay(Vector3.zero, source.transform.position, Color.green);
            }
      
            Vector3 reflection = Vector3.Reflect(inDirection.transform.position, source.right);

            Debug.DrawRay(source.transform.position, inDirection.transform.position, Color.green);
        }

        // RotateTowards
        // - поворачивает вектор в сторону цели
        // - функция аналогична MoveTowards, за исключением того, что вектор обрабатывается как направление, а не как позиция.
        // - вектор current будет повернут вокруг target направления на угол maxRadiansDelta, хотя он приземлится точно на цель, а не промахнется.
        // - если величины current и target различны, то величина результата будет линейно интерполирована во время вращения.
        // - Если используется отрицательное значение maxRadiansDelta, вектор будет вращаться от target до тех пор,
        // пока он не будет указывать точно в противоположном направлении, а затем остановится
        if (rotateTowardsVectors.Count == 2 && maxRadiansDelta != 0 && maxMagnitudeDelta != 0)
        {
            Transform current = rotateTowardsVectors[0].transform;
            Transform target = rotateTowardsVectors[1].transform;

            // Определяем направление вращения
            Vector3 targetDirection = target.position - current.position;

            // Поворот прямого вектора в направлении цели на один шаг
            Vector3 newDirection = Vector3.RotateTowards(current.forward, targetDirection, maxRadiansDelta, maxMagnitudeDelta);

            // Рисуем луч, указывающий на цель
            Debug.DrawRay(current.position, newDirection, Color.white, 2.0f);

            // Рассчитываем поворот на шаг ближе к цели и применяем поворот к этому объекту
            current.rotation = Quaternion.LookRotation(newDirection);

            var text = target.GetComponent<TextMeshPro>();
            text.text = $"RotateTowards:\nmaxRadiansDelta - {maxRadiansDelta}\nmaxMagnitudeDelta: {maxMagnitudeDelta}";
        }

        // Scale
        // - умножает два вектора покомпонентно
        if (scaleVectors.Count == 2)
        {
            Vector3 a = scaleVectors[0].transform.position;
            Vector3 b = scaleVectors[1].transform.position;


            Gizmos.color = Color.green;
            if (isScaleDrawVectors)
            {
                Gizmos.DrawLine(Vector3.zero, a);
                Gizmos.DrawLine(Vector3.zero, b);
            }

            Vector3 scale = Vector3.Scale(a, b);
            Debug.DrawRay(a, scale, Color.red, 3.0f);
        }

        // SignedAngle
        // - возвращает угол со знаком в градусах между fromи to.
        // - возвращается меньший из двух возможных углов между двумя векторами, поэтому результат никогда не будет больше 180 градусов или меньше - 180 градусов.
        // - если представить векторы «от» и «к» в виде линий на листе бумаги, исходящих из одной и той же точки, то axis вектор будет направлен вверх из бумаги.
        // - Измеренный угол между двумя векторами будет положительным по часовой стрелке и отрицательным против часовой стрелки.
        if (signedAngleVectors.Count == 2)
        {
            Transform from = signedAngleVectors[0].transform;
            Transform to = signedAngleVectors[1].transform;

            Gizmos.color = Color.green;

            if (isSignedAngleDrawVectors)
            {
                Gizmos.DrawLine(Vector3.zero, from.position);
                Gizmos.DrawLine(Vector3.zero, to.position);
            }

            Vector3 targetDir = to.position - from.position;
            Vector3 forward = from.forward;
            float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

            var text = from.GetComponent<TextMeshPro>();
            text.text = $"SignedAngle:\n{angle}";

        }

        // Slerp
        if (slerpVectors.Count == 2 && sLerpInterpolant != 0)
        {
            Transform a = slerpVectors[0].transform;
            Transform b = slerpVectors[1].transform;

            Vector3 slerp = Vector3.Slerp(a.position, b.position, sLerpInterpolant);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(slerp, 0.1f);

            if (isSlerpVectorsDrawVectors)
            {
                Gizmos.DrawLine(Vector3.zero, slerp);
            }

        }

        // SmoothDamp
        // - постепенно меняет вектор в сторону желаемой цели с течением времени.
        // - вектор сглаживается некоторой функцией, похожей на пружинный демпфер,
        // которая никогда не выйдет за пределы.
        // - чаще всего используется для сглаживания следящей камеры.
    }
}

// https://docs.unity3d.com/ScriptReference/Vector3.html
//Handles.DrawBezier(p1, p2, p1, p2, Color.green, null, 5);