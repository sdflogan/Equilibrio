using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
	public Mesh[] meshes;
	public Material material;

	public int maxDepth;
	public float childScale = 0.5f;

	public float spawnProbability;
	public float maxRotationSpeed;
	public float maxTwist;

	private float _rotationSpeed;
	private float _rotateRoot = 0f;

	private int _depth;

	private Material[,] materials;

	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f)
	};

	private void Start() {
		_rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
		transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

		if (materials == null)
			InitializeMaterials(); 

		gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer>().material = materials[_depth, Random.Range(0, 2)];
		if (_depth == 0) {
			transform.localScale *= 2f;
			_rotateRoot = 10f;
		} 
		if (_depth < maxDepth) {
			StartCoroutine(CreateChildren());
		}
		
	}

	private void Update() {
		transform.Rotate(_rotateRoot * Time.deltaTime, _rotationSpeed * Time.deltaTime, _rotateRoot * Time.deltaTime * 0.5f);
	}

	private void Initialize(Fractal parent, int childIndex) {
		meshes = parent.meshes;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		spawnProbability = parent.spawnProbability;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxTwist = parent.maxTwist;
		_depth = parent._depth + 1;
		transform.parent = parent.transform;
		childScale = parent.childScale;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
		if (_depth != 0) _rotateRoot = 0f;
	}

	private void InitializeMaterials() {
		materials = new Material[maxDepth + 1, 2];
		for (int i=0; i <= maxDepth; i++) {
			float t = i / (maxDepth - 1f);
			t *= t;
			materials[i, 0] = new Material(material);
			materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
			materials[i, 1] = new Material(material);
			materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
		}
		materials[maxDepth, 0].color = Color.magenta;
		materials[maxDepth, 1].color = Color.red;
	}

	IEnumerator CreateChildren() {
		for (int i=0; i<childDirections.Length; i++) {
			if (Random.value < spawnProbability) {
				yield return new WaitForSeconds(Random.Range(0.5f, 1f));
				new GameObject("Fractal Child").
					AddComponent<Fractal>().Initialize(this, i);
			}
		}
	}
}
