using System;
using System.Collections.Generic;
using Godot;

namespace Shooter.Shared
{
    internal class LoadingRequest
    {
        public string ResourceName { get; set; }
        public Action<Resource> OnSucess { get; set; }
    }

    /// <summary>
    /// Helper class to load resources in background
    /// </summary>
    public class AsyncLoader
    {
        [Signal]
        public event ProgressHandler OnProgress;
        public delegate void ProgressHandler(string filename, float percent);

        private Queue<LoadingRequest> resourceLoader = new Queue<LoadingRequest>();

        private LoadingRequest currentResource = null;

        /// <summary>
        /// Load an resource by given path
        /// </summary>
        /// <param name="resourceName">Path to resource file</param>
        /// <param name="callback">An action with returning an Resource/param>
        /// <returns></returns>
        public void LoadResource(string resourceName, Action<Resource> callback)
        {
            Logger.LogDebug(this, "Try to start load  " + resourceName);

            this.resourceLoader.Enqueue(new LoadingRequest
            {
                ResourceName = resourceName,
                OnSucess = callback
            });
        }

        /// <summary>
        /// Brings the async loader to the next state.
        /// Binded on your _Process Method
        /// </summary>
        public void Tick()
        {
            if (resourceLoader.Count > 0)
            {
                this.currentResource = resourceLoader.Dequeue();

                var result = ResourceLoader.LoadThreadedRequest(currentResource.ResourceName);
                if (result != Error.Ok)
                {
                    Logger.LogDebug(this, "Cant load resource " + currentResource.ResourceName + " - Reason: " + result.ToString());
                    this.currentResource = null;
                }
                else
                {
                    this.OnProgress?.Invoke(currentResource.ResourceName, 0);
                }
            }

            this.CheckLoad();
        }

        private void CheckLoad()
        {
            if (this.currentResource != null)
            {
                var mapLoaderProgress = new Godot.Collections.Array();
                if (currentResource != null)
                {
                    var status = ResourceLoader.LoadThreadedGetStatus(currentResource.ResourceName, mapLoaderProgress);
                    if (status == ResourceLoader.ThreadLoadStatus.Loaded)
                    {
                        Resource res = ResourceLoader.LoadThreadedGet(currentResource.ResourceName);
                        Logger.LogDebug(this, "Resource loaded " + currentResource.ResourceName);

                        if (currentResource.OnSucess != null)
                        {
                            currentResource.OnSucess(res);
                        }

                        currentResource = null;
                    }
                    else if (status == ResourceLoader.ThreadLoadStatus.InvalidResource || status == ResourceLoader.ThreadLoadStatus.Failed)
                    {
                        Logger.LogDebug(this, "Error loading  " + currentResource.ResourceName);
                        currentResource = null;
                    }
                    else if (status == ResourceLoader.ThreadLoadStatus.InProgress)
                    {
                        if (mapLoaderProgress.Count > 0)
                        {
                            this.OnProgress?.Invoke(currentResource.ResourceName, (float)mapLoaderProgress[0]);
                        }
                    }
                }
            }
        }

    }
}
