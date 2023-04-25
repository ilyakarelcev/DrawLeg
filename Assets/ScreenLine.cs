using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLine : MonoBehaviour
{

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _planeTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _pointer;

    [SerializeField] private List<Vector3> _positions = new();

    private Vector3 _oldPoint;

    [SerializeField] private float _step = 0.1f;
    [SerializeField] private Transform _borderQuad;
    [SerializeField] private LegsManager _legs;


    void Update()
    {
        Vector3 position = GetProjection();
        Vector3 positionInBorderSpace = _borderQuad.InverseTransformPoint(position);
        positionInBorderSpace.x = Mathf.Clamp(positionInBorderSpace.x ,- .5f, .5f);
        positionInBorderSpace.y = Mathf.Clamp(positionInBorderSpace.y, -.5f, .5f);

        position = _borderQuad.TransformPoint(positionInBorderSpace);

        _pointer.position = position;

        if (Input.GetMouseButtonDown(0))
        {
            
            _positions.Clear();
            _positions.Add(_planeTransform.InverseTransformPoint(position));
            _oldPoint = position;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _planeTransform.InverseTransformPoint(position));
            _lineRenderer.SetPosition(1, _planeTransform.InverseTransformPoint(position));
        }

        if (Input.GetMouseButton(0))
        {
            if (Vector3.Distance(position, _oldPoint) > _step)
            {
                _positions.Add(_planeTransform.InverseTransformPoint(position));
                _lineRenderer.positionCount = _positions.Count + 1;
                _lineRenderer.SetPositions(_positions.ToArray());

                _oldPoint = position;
            }

            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _planeTransform.InverseTransformPoint(position));
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_positions.Count > 1) {

                Vector3[] positionsWithOffset = new Vector3[_positions.Count];
                for (int i = 0; i < positionsWithOffset.Length; i++)
                {
                    positionsWithOffset[i] = _positions[i] - _positions[0];
                }

                _legs.UpdateStroke(positionsWithOffset);
            }
            
            _lineRenderer.positionCount = 0;
        }

    }

    private Vector3 GetProjection()
    {
        Plane plane = new Plane(_planeTransform.forward, _planeTransform.position);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        float distance;
        plane.Raycast(ray, out distance);
        Vector3 position = ray.GetPoint(distance);
        return position;
    }

}
