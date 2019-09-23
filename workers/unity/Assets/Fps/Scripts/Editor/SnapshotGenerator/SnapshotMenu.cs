using System;
using System.IO;
using Improbable;
using Improbable.Gdk.Core;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fps
{
    public class SnapshotMenu : MonoBehaviour
    {
        private static readonly string DefaultSnapshotPath =
            Path.Combine(Application.dataPath, "../../../snapshots/default.snapshot");

        private static readonly string CloudSnapshotPath =
            Path.Combine(Application.dataPath, "../../../snapshots/cloud.snapshot");

        private static readonly string SessionSnapshotPath =
            Path.Combine(Application.dataPath, "../../../snapshots/session.snapshot");

        [MenuItem("SpatialOS/Generate FPS Snapshot")]
        private static void GenerateFpsSnapshot()
        {
            SaveSnapshot(DefaultSnapshotPath, GenerateDefaultSnapshot());
            SaveSnapshot(CloudSnapshotPath, GenerateDefaultSnapshot());
            SaveSnapshot(SessionSnapshotPath, GenerateSessionSnapshot());
        }

        private static void AddHealthPacks(Snapshot snapshot)
        {
            var length = Random.Range(1, 100);
            for (int i = length - 1; i >= 0 ; i--)
            {
            // Invoke our static function to create an entity template of our health pack with 100 heath.
            var insideUnitSphere = (Vector3)(Random.insideUnitSphere* (2* length) );
            insideUnitSphere.y = 0;
            var healthPack = FpsEntityTemplates.HealthPickup(insideUnitSphere, (UInt32) Random.Range(1, 100));
            // Add the entity template to the snapshot.
            snapshot.AddEntity(healthPack);
                
            }

        }

        private static Snapshot GenerateDefaultSnapshot()
        {
            var snapshot = new Snapshot();
            snapshot.AddEntity(FpsEntityTemplates.Spawner(Coordinates.Zero));
            AddHealthPacks(snapshot);
            return snapshot;
        }

        private static Snapshot GenerateSessionSnapshot()
        {
            var snapshot = new Snapshot();
            snapshot.AddEntity(FpsEntityTemplates.Spawner(Coordinates.Zero));
            snapshot.AddEntity(FpsEntityTemplates.DeploymentState());
            AddHealthPacks(snapshot);
            return snapshot;
        }

        private static void SaveSnapshot(string path, Snapshot snapshot)
        {
            snapshot.WriteToFile(path);
            Debug.LogFormat("Successfully generated initial world snapshot at {0}", path);
        }
    }
}
