using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyEditor : EditorWindow
{
    [MenuItem("Data/EditorData")]
    public static void ShowDataWindow()
    {
        var wnd = GetWindow<KeyEditor>();
        wnd.titleContent = new GUIContent("Data editor settings");
    }

    private string project_id;
    private string private_key_id;
    private string private_key;
    private string client_email;
    private string client_id;
    
    private string FolderToSave;
    
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        Label label = new Label("project_id");
        root.Add(label);
        TextField textField = new TextField();
        textField.SetValueWithoutNotify(EditorPrefs.GetString("project_id"));
        root.Add(textField);
        textField.RegisterValueChangedCallback(newValue => project_id = newValue.newValue);
        
        Label label2 = new Label("private_key_id");
        root.Add(label2);
        TextField textField2 = new TextField();
        textField2.SetValueWithoutNotify(EditorPrefs.GetString("private_key_id"));
        root.Add(textField2);
        textField2.RegisterValueChangedCallback(newValue => private_key_id = newValue.newValue);

        Label label3 = new Label("private_key");
        root.Add(label3);
        TextField textField3 = new TextField();
        textField3.SetValueWithoutNotify(EditorPrefs.GetString("private_key"));
        root.Add(textField3);
        textField3.RegisterValueChangedCallback(newValue => private_key = newValue.newValue);

        Label label4 = new Label("client_email");
        root.Add(label4);
        TextField textField4 = new TextField();
        textField4.SetValueWithoutNotify(EditorPrefs.GetString("client_email"));
        root.Add(textField4);
        textField4.RegisterValueChangedCallback(newValue => client_email = newValue.newValue);

        Label label5 = new Label("client_id");
        root.Add(label5);
        TextField textField5 = new TextField();
        textField5.SetValueWithoutNotify(EditorPrefs.GetString("client_id"));
        root.Add(textField5);
        textField5.RegisterValueChangedCallback(newValue => client_id = newValue.newValue);
        
        Label label6 = new Label("FolderToSave");
        root.Add(label6);
        TextField textField6 = new TextField();
        textField6.SetValueWithoutNotify(EditorPrefs.GetString("FolderToSave"));
        root.Add(textField6);
        textField6.RegisterValueChangedCallback(newValue => FolderToSave = newValue.newValue);

        
        Button button = new Button
        {
            name = "Valider",
            text = "Valider"
        };
        button.clickable.clicked += GenerateSettingsFile;
        root.Add(button);
    }

    private void GenerateSettingsFile()
    {
        EditorPrefs.SetString("project_id",project_id);
        EditorPrefs.SetString("private_key_id",private_key_id);
        EditorPrefs.SetString("private_key",private_key);
        EditorPrefs.SetString("client_email",client_email);
        EditorPrefs.SetString("client_id",client_id);
        EditorPrefs.SetString("FolderToSave",FolderToSave);
        GenerateJsonKeys();
    }
    
    private void GenerateJsonKeys()
    {
        var json = new StringBuilder();
        json.AppendLine("{");
        json.AppendLine("\"type\": \"service_account\",");
        json.AppendLine($"\"project_id\": \"{project_id}\",");
        json.AppendLine($"\"private_key_id\": \"{private_key_id}\",");
        json.AppendLine($"\"private_key\": \"{private_key}\",");
        json.AppendLine($"\"client_email\": \"{client_email}\",");
        json.AppendLine($"\"client_id\": \"{client_id}\",");
        json.AppendLine($"\"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",");
        json.AppendLine($"\"token_uri\": \"https://oauth2.googleapis.com/token\",");
        json.AppendLine($"\"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",");
        json.AppendLine($"\"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/data-146%40testdata-479504.iam.gserviceaccount.com\",");
        json.AppendLine($"\"universe_domain\": \"googleapis.com\"");
        json.AppendLine("}");

        string filePath = $"{FolderToSave}/service-account.json";
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        File.WriteAllText(filePath, json.ToString());
    }
}
