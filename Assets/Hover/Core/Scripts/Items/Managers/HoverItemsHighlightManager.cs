﻿using System;
using System.Collections.Generic;
using Hover.Core.Cursors;
using UnityEngine;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsHighlightManager : MonoBehaviour {

		public HoverCursorDataProvider CursorDataProvider;

		private List<HoverItemHighlightState> vHighStates;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = FindObjectOfType<HoverCursorDataProvider>();
			}

			if ( CursorDataProvider == null ) {
				throw new ArgumentNullException("CursorDataProvider");
			}

			vHighStates = new List<HoverItemHighlightState>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();
			
			itemsMan.FillListWithExistingItemComponents(vHighStates);
			ResetItems();
			UpdateItems();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void ResetItems() {
			for ( int i = 0 ; i < vHighStates.Count ; i++ ) {
				HoverItemHighlightState highState = vHighStates[i];
				
				if ( highState == null ) {
					vHighStates.RemoveAt(i);
					i--;
					Debug.LogWarning("Found and removed a null item; use RemoveItem() instead.");
					continue;
				}
				
				highState.ResetAllNearestStates();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateItems() {
			List<ICursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;
			
			for ( int i = 0 ; i < cursorCount ; i++ ) {
				ICursorData cursor = cursors[i];
				cursor.MaxItemHighlightProgress = 0;
				cursor.MaxItemSelectionProgress = 0;

				if ( !cursor.CanCauseSelections ) {
					continue;
				}

				HoverItemHighlightState.Highlight? high;
				HoverItemHighlightState highState = FindNearestItemToCursor(cursor.Type, out high);

				if ( highState == null || high == null ) {
					continue;
				}

				highState.SetNearestAcrossAllItemsForCursor(cursor.Type);
				cursor.MaxItemHighlightProgress = high.Value.Progress;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverItemHighlightState FindNearestItemToCursor(CursorType pCursorType, 
												out HoverItemHighlightState.Highlight? pNearestHigh) {
			float minDist = float.MaxValue;
			HoverItemHighlightState nearestItem = null;

			pNearestHigh = null;
			
			for ( int i = 0 ; i < vHighStates.Count ; i++ ) {
				HoverItemHighlightState item = vHighStates[i];
				
				if ( !item.gameObject.activeInHierarchy || item.IsHighlightPrevented ) {
					continue;
				}
				
				HoverItemHighlightState.Highlight? high = item.GetHighlight(pCursorType);
				
				if ( high == null || high.Value.Distance >= minDist ) {
					continue;
				}
				
				minDist = high.Value.Distance;
				nearestItem = item;
				pNearestHigh = high;
			}
			
			return nearestItem;
		}

	}

}
