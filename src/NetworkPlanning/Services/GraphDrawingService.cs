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
        private const int StartY = 30;
        private const int StartX = 30;
        private const int DeltaX = 120;
        private const int DeltaY = 85;
        private readonly IUnityContainer _container;

        private readonly Style _criticalLineStyle =
            Application.Current.FindResource("CriticalLineStyle") as Style;

        private readonly Style _defaultLineStyle =
            Application.Current.FindResource("LineStyle") as Style;

        private readonly double _ellipseRadius;
        private readonly ObservableCollection<EventViewModel> _events;
        private MainViewModel _mainViewModel;

        public GraphDrawingService(
            IUnityContainer container,
            NetworkObjectProvider objectProvider)
        {
            container.RegisterInstance(this);
            _container = container;
            _events = objectProvider.Events;
            _ellipseRadius = EllipseWidth / 2;
        }

        private MainViewModel MainViewModel =>
            _mainViewModel ?? (_mainViewModel = _container.Resolve<MainViewModel>());

        public void DrawGraph(Canvas canvas)
        {
            MainViewModel.CriticalPath = string.Empty;
            MainViewModel.OtherWays = string.Empty;
            canvas.Children.Clear();
            var checkedNodes = new List<EventViewModel>();
            var x = StartX;
            var y = StartY;
            var group = _events.Where(e => e.InWorks.Count == 0).ToList();
            checkedNodes = checkedNodes.Concat(group).ToList();

            while (checkedNodes.Count != _events.Count)
            {
                DrawGroup(canvas, group, x, ref y);

                group = _events
                    .Where(e => !checkedNodes.Contains(e) && e.InWorks.Count > 0 &&
                                checkedNodes
                                    .Intersect(e.InWorks.Select(w => w.StartEvent))
                                    .Count() == e.InWorks.Count)
                    .ToList();

                checkedNodes = checkedNodes.Concat(group).ToList();
                y = StartY;
                x += DeltaX;
            }

            DrawGroup(canvas, group, x, ref y);

            foreach (var @event in _events)
            {
                DrawLines(canvas, @event);
            }

            if (_events.Count > 0)
            {
                var ways = new List<EventViewModel[]>();
                FindWays(_events.FirstOrDefault(), ref ways);

                if (ways.Count > 0)
                {
                    FindCriticalPath(ways);
                }
            }
        }

        private void DrawLines(Canvas canvas, EventViewModel @event)
        {
            foreach (var work in @event.OutWorks)
            {
                var line = work.Line;

                if (!canvas.Children.Contains(line))
                {
                    canvas.Children.Add(line);
                }

                if (!canvas.Children.Contains(work.LineText))
                {
                    canvas.Children.Add(work.LineText);
                }

                line.X1 = @event.Left + EllipseWidth;
                line.X2 = work.EndEvent.Left;
                line.Y1 = @event.Top + _ellipseRadius;
                line.Y2 = work.EndEvent.Top + _ellipseRadius;
                line.Style = _defaultLineStyle;
                work.ArrangeLineText();
            }
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

            var ellipse = new Ellipse
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

            grid.Children.Add(ellipse);
            grid.Children.Add(text);
            Interaction.GetBehaviors(grid).Add(new UiElementDragBehavior {Ellipse = ellipse});
            return grid;
        }

        private void FindWays(EventViewModel node, ref List<EventViewModel[]> ways)
        {
            if (node.OutWorks.Count == 0)
            {
                return;
            }

            var tail = ways.FirstOrDefault(x => x[x.Length - 1].Equals(node)) ?? new EventViewModel[0];
            ways.Remove(tail);

            foreach (var work in node.OutWorks)
            {
                ways.Add(tail
                    .Except(new[] {tail.LastOrDefault()})
                    .Concat(new[] {node, work.EndEvent})
                    .ToArray());
                FindWays(work.EndEvent, ref ways);
            }
        }

        private void FindCriticalPath(List<EventViewModel[]> ways)
        {
            var criticalPath = ways[0];
            var maxDuration = 0;
            var criticalPathString = "";
            var waysString = "";

            foreach (var path in ways)
            {
                var duration = CalculatePathDuration(path, out var str);
                waysString += str + '\n';

                if (duration >= maxDuration)
                {
                    maxDuration = duration;
                    criticalPath = path;
                    criticalPathString = str;
                }
            }

            for (var i = 0; i < criticalPath.Length - 1; i++)
            {
                criticalPath[i].OutWorks
                    .First(x => x.EndEvent.Equals(criticalPath[i + 1]))
                    .Line.Style = _criticalLineStyle;
            }

            var index = waysString.IndexOf(criticalPathString);
            waysString = waysString.Remove(index, criticalPathString.Length + 1);
            MainViewModel.CriticalPath = criticalPathString;
            MainViewModel.OtherWays = waysString;
        }

        private int CalculatePathDuration(EventViewModel[] path, out string str)
        {
            str = "";
            var duration = 0;

            for (var i = 0; i < path.Length - 1; i++)
            {
                duration += path[i].OutWorks
                    .First(x => x.EndEvent.Equals(path[i + 1])).Duration;

                str += path[i] + "-";
            }

            str += path[path.Length - 1] + " = " + duration;
            return duration;
        }
    }
}