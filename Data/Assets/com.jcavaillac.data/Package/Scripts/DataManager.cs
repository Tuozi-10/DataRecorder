using System;
using UnityEngine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;


public class DataManager : MonoBehaviour
{
   private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
      private SheetsService service;
      
      public string spreadsheetId = "1jDJKW1bMuYCcQfJ5Ozomasuv2psPPmtFEKVgBrD-3zk";  // ID Google Sheets
      public string sheetName = "Feuille_1";

      public static DataManager Instance;

      private void Awake()
      {
          if (Instance != null)
          {
              Destroy(this);
              return;
          }

          DontDestroyOnLoad(this);
          Instance = this;
      }

      private async void Start()
      {
          await InitService();
      }
      
      private async Task InitService()
      {
          TextAsset jsonFile = Resources.Load<TextAsset>("service-account");
          GoogleCredential credential;
  
          using (var stream = new MemoryStream(jsonFile.bytes))
          {
              credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
          }
  
          service = new SheetsService(new BaseClientService.Initializer()
          {
              HttpClientInitializer = credential,
              ApplicationName = "UnityGoogleSheets",
          });
  
          Debug.Log("Google Sheets prêt !");
      }

      public async Task UpdateCell(string cell, object newValue)
      {
          var valueRange = new ValueRange
          {
              Values = new List<IList<object>>
              {
                  new List<object> { newValue }
              }
          };

          string range = $"{sheetName}!{cell}";

          var updateRequest = service.Spreadsheets.Values.Update(
              valueRange,
              spreadsheetId,
              range
          );

          updateRequest.ValueInputOption =
              SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

          await updateRequest.ExecuteAsync();

          Debug.Log($"Cellule {cell} mise à jour !");
      }
      
      public async Task<int> ReadIntCell(string cell)
      {
          string range = $"{sheetName}!{cell}";

          var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
          var response = await request.ExecuteAsync();

          if (response.Values == null || response.Values.Count == 0)
          {
              Debug.LogWarning($"Cellule {cell} vide, valeur = 0");
              return 0;
          }

          string rawValue = response.Values[0][0].ToString();

          if (int.TryParse(rawValue, out int result))
              return result;

          Debug.LogWarning($"Impossible de parser {rawValue}, valeur = 0");
          return 0;
      }
      
      public async Task IncrementCell(string cell, int increment = 1)
      {
          int currentValue = await ReadIntCell(cell);
          int newValue = currentValue + increment;

          await UpdateCell(cell, newValue);

          Debug.Log($"Cellule {cell} : {currentValue} → {newValue}");
      }
      
}
