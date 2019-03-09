using UnityEngine;
using UnityEngine.UI;
public class ScaleAnimationHelper : MonoBehaviour
{
    private Image _image;
    private SpriteRenderer _sprite;
    private Vector2 _originSzie;
    public float scale = 1.1f;
    public float speed = 4f;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();

        if (_image != null)
        {
            _originSzie = _image.rectTransform.sizeDelta;    
        }

        if (_sprite != null)
        {
            _originSzie = new Vector2(_sprite.transform.localScale.x, _sprite.transform.localScale.y);    
        }
    }

    private void Update()
    {
        float lerpValue = Mathf.PingPong(Time.time  * speed, 1f);
        if (_image != null)
        {
            _image.rectTransform.sizeDelta = Vector2.Lerp(
                new Vector2(_originSzie.x, _originSzie.y), 
                scale * new Vector2(_originSzie.x, _originSzie.y),
                lerpValue
            );    
        }

        if (_sprite != null)
        {
            _sprite.transform.localScale = Vector2.Lerp(
                _originSzie,
                scale * _originSzie,
                lerpValue
            ); 
        }
    }
}