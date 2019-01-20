/**
 * Author:    Aaron Moffatt
 * Created:   01.19.2019
 * 
 * MIT Reality Virtually Hackathon
 **/

using UnityEngine;

using System.Collections.Generic;
using System.IO;


namespace IronManUI {

    [System.Serializable]
    public class SlideModel {
        public string name;
        public Vector3 position;
        public List<IMComponentModel> components = new List<IMComponentModel>();
    }

    [System.Serializable]
    public class PresentationModel {
        public string name = "Untitled";
        public List<SlideModel> slides = new List<SlideModel>();


        public string SaveToJson() {
            return JsonUtility.ToJson(this, true);
        }

        public void LoadFromJson(string json) {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    public class PresentationManager : MonoBehaviour {

        public static PresentationManager instance { get; private set; }

        public string presentationFile = "Assets/Resources/Presentations/Main Presentation.pxr";
        public string presentationName = "Untitled";

        public Slide slidePrefab;
        public TextBox textPrefab;
        public ThreeDItem threeDPrefab;

        private Slide _currentSlide;
        public Slide currentSlide {
            get {
                return _currentSlide;
            }
            set {
                if (_currentSlide == value)
                    return;

                if (_currentSlide != null) {
                    _currentSlide.Deactivate();
                }
                _currentSlide = value;
                _currentSlide.Activate();
            }
        }

        // private PresentationModel model;

        void OnEnable() {
            if (instance != null && instance != this)
                Debug.LogWarning("Having multiple presentations in a scene is not supported");
            instance = this;

            Load();
        }

        public void Load() {
            string json = File.ReadAllText(presentationFile);

            PresentationModel model;
            if (json != null && json.Length > 0) {
                model = new PresentationModel();
                model.LoadFromJson(json);
            } else {
                model = MakeDefault();
            }
            SetModel(model);
        }

        public void Save() {
            var model = ExtractModel();
            string json = model.SaveToJson();
            File.WriteAllText(presentationFile, json);
        }

        protected void SetModel(PresentationModel model) {
            presentationName = model.name;

            GetComponentsInChildren<Slide>().DestroyAllGameObjects();

            foreach (var slideModel in model.slides) {
                var slideObject = Instantiate(slidePrefab);
                slideObject.SetModel(this, slideModel);
                slideObject.transform.parent = transform;
            }
        }

        protected PresentationModel ExtractModel() {
            var model = new PresentationModel();
            model.name = presentationName;

            foreach (var slide in GetComponentsInChildren<Slide>()) {
                model.slides.Add(slide.ExtractModel());
            }

            return model;
        }

        public PresentationModel MakeDefault() {
            var model = new PresentationModel();
            
            var slide1 = new SlideModel();
            slide1.name = "Test slide 1";
            model.slides.Add(slide1);

            var slide2 = new SlideModel();
            slide2.name = "Test slide 2";
            model.slides.Add(slide2);

            return model;
        }

        public AbstractIMComponent InstantiateComponent(IMComponentModel model) {
            if (model == null) {
                Debug.LogWarning("Cannot create component for null model");
                return null;
            }

            if (model is TextBoxModel) {
                var textbox = Instantiate(textPrefab) as TextBox;
                textbox.model = model;
            } else if (model is ThreeDItemModel) {
                var item = Instantiate(threeDPrefab);
                item.model = model;
            }

            Debug.LogWarning("Cannot create component for model: " + model.GetType());
            return null;
        }

        public void SlideWaypointTouched(Slide slide) {
            if (slide == null)
                return;

            currentSlide = slide;
        }


    }
}