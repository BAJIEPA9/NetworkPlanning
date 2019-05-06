using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Shapes;
using NetworkPlanning.Behaviour;
using NetworkPlanning.ViewModels;
using Unity;

namespace NetworkPlanning.Services
{
    internal class GraphDrawingService
    {
        private const double EllipseWidth = 60;
        private readonly double _ellipseRadius;
        private const int StartY = 30;
        private const int StartX = 30;
        private const int DeltaX = 120;
        private const int DeltaY = 85;
        private readonly ObservableCollection<EventViewModel> _events;

        public GraphDrawingService(
            IUnityContainer container,
            NetworkObjectProvider objectProvider)
        {
            container.RegisterInstance(this);
            _events = objectProvider.Events;
            _ellipseRadius = EllipseWidth / 2;
        }

        public void DrawGraph(Canvas canvas)
        {
            canvas.Children.Clear();
            var checkedNodes = new List<EventViewModel>();
            var x = StartX;
            var y = StartY;
            var group = _events.Where(e => e.Works.Count == 0).ToList();
            checkedNodes = checkedNodes.Concat(group).ToList();

            while (checkedNodes.Count != _events.Count)
            {
                DrawGroup(canvas, group, x, ref y);

                group = _events
                    .Where(e => !checkedNodes.Contains(e) && e.Works.Count > 0 &&
                                checkedNodes
                                    .Select(l => (int?) l.EventNumber)
                                    .Intersect(e.Works.Select(w => w.StartEvent))
                                    .Count() == e.Works.Count)
                    .ToList();

                checkedNodes = checkedNodes.Concat(group).ToList();
                y = StartY;
                x += DeltaX;
            }

            DrawGroup(canvas, group, x, ref y);

            foreach (var @event in _events)
            {
                @event.OutLines.Clear();
                @event.InLines.Clear();
            }

            foreach (var @event in _events)
            {
                DrawLines(canvas, @event);
            }
        }

        private void DrawLines(Canvas canvas, EventViewModel @event)
        {
            var nodes = _events
                .Where(x => x.Works.Select(w => w.StartEvent).Contains(@event.EventNumber))
                .ToArray();

            foreach (var node in nodes)
            {
                var line = DrawLine(canvas, @event, node);
                @event.OutLines.Add(line);
                node.InLines.Add(line);
            }
        }

        private Line DrawLine(Canvas canvas, EventViewModel left, EventViewModel right)
        {
            var line = new Line
            {
                X1 = left.Left + EllipseWidth,
                X2 = right.Left,
                Y1 = left.Top + _ellipseRadius,
                Y2 = right.Top + _ellipseRadius
            };

            canvas.Children.Add(line);

            return line;
        }

        private void DrawGroup(Canvas canvas, IEnumerable<EventViewModel> group, int x, ref int y)
        {
            foreach (var @event in group)
            {
                @event.Left = x;
                @event.Top = y;
                var node = CreateNode(@event.EventNumber);
                node.DataContext = @event;
                canvas.Children.Add(node);
                Canvas.SetLeft(node, x);
                Canvas.SetTop(node, y);
                y += DeltaY;
            }
        }

        private Grid CreateNode(int eventNumber)
        {
            var grid = new Grid();

            var el = new Ellipse
            {
                Width = EllipseWidth,
                Height = EllipseWidth
            };

            var text = new TextBlock
            {
                Text = eventNumber.ToString(),
                FontWeight = FontWeights.Bold,
                FontSize = 30
            };

            grid.Children.Add(el);
            grid.Children.Add(text);
            Interaction.GetBehaviors(grid).Add(new UiElementDragBehavior());
            return grid;
        }
    }
}