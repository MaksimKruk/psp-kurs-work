using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using PSP.GameCommon;
using PSP.GameCommon.GameObjects;

namespace PSP.GameClient.ApiCaller
{
    public class NetworkService : INetworkService
    {
        private HttpClient _httpClient;

        public NetworkService()
        {
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("http://localhost/RaceGame.Api/api");
            //_httpClient.BaseAddress = new Uri("http://172.20.10.2/RaceGame.Api/api");
            _httpClient.BaseAddress = new Uri("http://localhost:5000");

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void GetShot(string clientId)
        {
            _ = _httpClient.PutAsJsonAsync<object>($"api/gamer/{clientId}/shot", null).Result;
        }

        public Bullet[] GetBullets()
        {
            var response = _httpClient.GetAsync($"api/gamer/bullets").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<Bullet[]>(content);
        }

        public void ResetGame()
        {
            _ = _httpClient.PutAsJsonAsync<object>($"api/game-object/reset", null).Result;
        }

        public Car GetEnemyGamer(string clientId)
        {
            var response = _httpClient.GetAsync($"api/gamer/{clientId}/enemy").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<Car>(content);
        }

        public List<GameObject> GetGameObjects(string gamerId)
        {
            var response = _httpClient.GetAsync($"api/game-object/{gamerId}/all").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GameObject>>(content);
        }

        public List<GameObject> GetLevelRightSequence()
        {
            var response = _httpClient.GetAsync($"api/game-object/level/right").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GameObject>>(content);
        }

        public List<GameObject> GetLevel()
        {
            var response = _httpClient.GetAsync($"api/game-object/level").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GameObject>>(content);
        }

        public List<Car> GetCars()
        {
            var response = _httpClient.GetAsync($"api/gamer").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<Car>>(content);
        }

        public GameObject[] GetPrizes()
        {
            var response = _httpClient.GetAsync($"api/game-object/prizes").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GameObject[]>(content);
        }

        public Point[] GetPrizesState()
        {
            var response = _httpClient.GetAsync($"api/game-object/prizes/state").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<Point[]>(content);
        }

        public Car CreateGamer(string clientId)
        {
            var response = _httpClient.PostAsJsonAsync($"api/gamer", clientId).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            Car result = JsonConvert.DeserializeObject<Car>(content);

            return result;
        }

        public Car MoveGamer(string gamerId, int direction)
        {
            var response = _httpClient
                .PutAsJsonAsync($"api/gamer/{gamerId}/move/{direction}", "").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<Car>(content);
        }

        public void DeleteGamer(string gamerId)
        {
            _ = _httpClient.DeleteAsync($"api/gamer/{gamerId}").Result;
        }

        public void UpdateGamerTexture(Car car)
        {
            _ = _httpClient.PutAsJsonAsync<Car>($"api/gamer/texture", car).Result;
        }
    }
}
