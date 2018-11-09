// AutoNet
// Class1.cs
// 
// ============================================================
// 
// Created: 2018-11-09
// Last Updated: 2018-11-09-10:32 AM
// By: Adam Renaud
// 
// ============================================================

using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace AutoListAC
{
    public static class AutoNet
    {
        /// <summary>
        ///     The AutoLength function is used to
        ///     Get the total length of all Polylines and
        ///     Lines in a selection
        /// </summary>
        [CommandMethod("AutoLength", CommandFlags.UsePickSet)]
        public static void AutoLength()
        {
            // Get the document and the database
            var acDocument = Application.DocumentManager.MdiActiveDocument;
            var acCurrentDatabase = acDocument.Database;

            // Start the transaction
            using ( var acTransaction = acCurrentDatabase.TransactionManager.StartTransaction() )
            {
                // Selection options to display a prompt to the user
                // during selection
                var selectionOptions = new PromptSelectionOptions
                {
                    MessageForAdding = "Select lines and polylines that you would like to sum: "
                };

                var promptResult = acDocument.Editor.SelectImplied();
                acDocument.Editor.WriteMessage(promptResult.Status.ToString());

                // Prompt the user for selection
                if ( promptResult.Status != PromptStatus.OK || promptResult.Value.Count == 0 )
                    promptResult = acDocument.Editor.GetSelection(selectionOptions);

                if ( promptResult.Status != PromptStatus.OK ) return;
                var selection = promptResult.Value;

                // The lengths of the selected objects
                var lengths = new List<double>();

                foreach ( SelectedObject selectedObject in selection )
                {
                    if ( selectedObject == null ) continue;
                    var entity = acTransaction.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;

                    // Check for null
                    if ( entity == null ) continue;

                    switch ( entity.ObjectId.ObjectClass.DxfName )
                    {
                        // if the entity is a polyline then continue
                        case "LWPOLYLINE":
                            var polyline = entity as Polyline;

                            // Make sure that the polyline is not null
                            if ( polyline == null ) continue;
                            lengths.Add(polyline.Length);
                            break;

                        case "LINE":
                            var line = entity as Line;

                            // Make sure that the line is not null
                            if ( line == null ) continue;

                            lengths.Add(line.Length);
                            break;

                        default:
                            continue;
                    }
                }

                // Print out the total sum of all lengths
                acDocument.Editor.WriteMessage($"\nTotal Length of all lines: {lengths.Sum()} units.");
            }
        }
    }
}