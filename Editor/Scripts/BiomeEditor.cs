﻿using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using LibNoise;
using LibNoise.Models;
using LunraGames.NoiseMaker;
using LunraGames;

namespace LunraGamesEditor.NoiseMaker
{
	public static class BiomeEditor
	{
		public static int PreviewWidth = 198;
		public static int PreviewHeight = 64;

		public static VisualizationPreview Previewer = NodeEditor.Visualizations[0];

		static List<BiomePreview> Previews = new List<BiomePreview>();

		public static BiomePreview GetPreview(Mercator mercator, Domain domain, Biome biome, object module)
		{
			var preview = Previews.FirstOrDefault(p => p.Id == biome.Id && p.DomainId == domain.Id);

			if (preview == null)
			{
				preview = new BiomePreview { Id = biome.Id, DomainId = domain.Id, Stale = true };
				preview.Preview = new Texture2D(PreviewWidth, PreviewHeight);
				Previews.Add(preview);
			}
			else
			{
				preview.Stale = preview.Stale || biome.AltitudeIds.Count != preview.LastSourceIds.Count || preview.LastVisualizer != Previewer;
				for (var i = 0; i < biome.AltitudeIds.Count; i++)
				{
					var id = biome.AltitudeIds[i];
					preview.Stale = preview.Stale || id != preview.LastSourceIds[i];
				}
				preview.Stale = preview.Stale || preview.LastUpdated < DomainEditor.LastUpdated(domain.Id);
			}

			if (preview.Stale)
			{
				var width = preview.Preview.width;
				var height = preview.Preview.height;
				var pixels = new Color[width * height];

				Thrifty.Queue(
					() =>
					{
						for (var x = 0; x < width; x++)
						{
							for (var y = 0; y < height; y++)
							{
								float value;
								float weight;
								Color color;

								var isSphere = module is Sphere;

								if (isSphere)
								{
									var latitude = SphereUtils.GetLatitude(y, height);
									var longitude = SphereUtils.GetLongitude(x, width);
									value = (float)(module as Sphere).GetValue(latitude, longitude);
									weight = domain.GetSphereWeight(latitude, longitude, value);
									color = biome.GetSphereColor(latitude, longitude, value, mercator);
								}
								else
								{
									value = (float)(module as IModule).GetValue(x, y, 0f);
									weight = domain.GetPlaneWeight(x, y, value);
									color = biome.GetPlaneColor(x, y, value, mercator);
								}

								var hiddenColor = Previewer.Calculate(value, Previewer);

								pixels[Texture2DExtensions.PixelCoordinateToIndex(x, y, width, height)] = Mathf.Approximately(0f, weight) ? hiddenColor : color;
							}
						}
					},
					() => TextureFarmer.Queue
					(
						preview.Preview, 
						pixels, 
						() =>
						{
							MercatorMakerWindow.PreviewUpdating = false;
							MercatorMakerWindow.QueueRepaint();
						},
						() =>
						{
							MercatorMakerWindow.PreviewUpdating = true;
							MercatorMakerWindow.QueueRepaint();
						}
					),
					Debug.LogException
				);

				preview.Stale = false;
				preview.LastUpdated = DateTime.Now.Ticks;
				preview.LastSourceIds = new List<string>(biome.AltitudeIds);
				preview.LastVisualizer = Previewer;
				preview.LastModule = module;
			}

			return preview;
		}
	}
}