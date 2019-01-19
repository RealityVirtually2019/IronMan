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
    public class PresentationModel : ScriptableObject {
        public string presentationName = "Untitled";
        public List<SlideModel> slides = new List<SlideModel>();


        public void SaveToFile(string filepath) {
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(filepath, json);
        }

        public void LoadFromFile(string filepath) {
            string json = File.ReadAllText(filepath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    public class PresentationManager : MonoBehaviour {
        public string presentationFile = "Assets/Resources/Presentations/Main Presentation.pxr";

        public Slide slidePrefab;
        public TextBox textPrefab;



        // private PresentationModel model;

        void OnEnable() {
            Load();
        }

        public void Load() {
            var model = new PresentationModel();
            model.LoadFromFile(presentationFile);
            SetModel(model);
        }

        public void Save() {
            var model = ExtractModel();
            model.SaveToFile(presentationFile);
        }

        protected void SetModel(PresentationModel model) {
            GetComponentsInChildren<Slide>().DestroyAllGameObjects();

            foreach (var slideModel in model.slides) {
                var slideObject = Instantiate(slidePrefab);
                slideObject.SetModel(slideModel);
                slideObject.transform.parent = transform;
            }
        }

        protected PresentationModel ExtractModel() {
            var model = new PresentationModel();
            model.name = gameObject.name;

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
            }

            Debug.LogWarning("Cannot create component for model: " + model.GetType());
            return null;
        }


    }
}