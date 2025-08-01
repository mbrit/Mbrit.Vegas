import 'package:flutter/material.dart';
import 'package:flutter_markdown/flutter_markdown.dart';
import 'dart:convert';
import 'dart:io';
import 'package:flutter/services.dart';
import 'main.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mbrit_vegas/setup_run_page.dart';

class TutorialPage extends StatefulWidget {
  final bool fromStartWalk;
  const TutorialPage({Key? key, this.fromStartWalk = false}) : super(key: key);

  @override
  State<TutorialPage> createState() => _TutorialPageState();
}

class _TutorialPageState extends State<TutorialPage>
    with TickerProviderStateMixin {
  int _currentSlideIndex = 0;
  double _videoProgress = 0.0;
  bool _isVideoPlaying = false;
  List<TutorialSlide> _slides = [];
  bool _isLoading = true;
  final ScrollController _scrollController = ScrollController();
  AnimationController? _dotAnimationController;
  Animation<double>? _dotAnimation;
  Animation<double>? _dotOpacityAnimation;

  @override
  void initState() {
    super.initState();
    _loadTutorialSlides();

    // Initialize dot animation
    _dotAnimationController = AnimationController(
      duration: const Duration(milliseconds: 1800),
      vsync: this,
    );
    _dotAnimation = Tween<double>(begin: 0.0, end: -100.0).animate(
      CurvedAnimation(
        parent: _dotAnimationController!,
        curve: Curves.easeInOut,
      ),
    );

    // Create opacity animation with fade in and fade out
    _dotOpacityAnimation = TweenSequence<double>([
      TweenSequenceItem(
        tween: Tween<double>(begin: 0.0, end: 1.0),
        weight: 1.0,
      ), // Fade in over first 15% (270ms)
      TweenSequenceItem(
        tween: Tween<double>(begin: 1.0, end: 1.0),
        weight: 6.7,
      ), // Stay visible for most of animation
      TweenSequenceItem(
        tween: Tween<double>(begin: 1.0, end: 0.0),
        weight: 1.0,
      ), // Fade out over last 15% (270ms)
    ]).animate(_dotAnimationController!);

    // Start the animation when the page loads
    WidgetsBinding.instance.addPostFrameCallback((_) {
      print('Starting dot animation');
      _dotAnimationController?.repeat(count: 2);
    });
  }

  @override
  void dispose() {
    _scrollController.dispose();
    _dotAnimationController?.dispose();
    super.dispose();
  }

  Future<void> _loadTutorialSlides() async {
    final List<TutorialSlide> slides = [];

    for (int i = 1; i <= 17; i++) {
      final slideNumber = i.toString().padLeft(2, '0');
      final markdownContent = await _loadMarkdownFile(
        'assets/tutorial/slide_$slideNumber.md',
      );
      final metadata = await _loadMetadataFile(
        'assets/tutorial/slide_$slideNumber.json',
      );

      slides.add(
        TutorialSlide(
          title: _getTitleFromMarkdown(markdownContent),
          content: markdownContent,
          videoTimestamp: _getVideoTimestamp(i),
          metadata: metadata,
        ),
      );
    }

    setState(() {
      _slides = slides;
      _isLoading = false;
    });
  }

  Future<String> _loadMarkdownFile(String path) async {
    try {
      return await rootBundle.loadString(path);
    } catch (e) {
      return '# Error Loading Content\n\nCould not load tutorial content.';
    }
  }

  Future<Map<String, dynamic>> _loadMetadataFile(String path) async {
    try {
      final jsonString = await rootBundle.loadString(path);
      return json.decode(jsonString);
    } catch (e) {
      return {};
    }
  }

  String _getTitleFromMarkdown(String markdown) {
    final lines = markdown.split('\n');
    for (final line in lines) {
      if (line.startsWith('# ') && !line.startsWith('## ')) {
        return line.substring(2).trim();
      }
    }
    return 'Tutorial Slide';
  }

  double _getVideoTimestamp(int slideIndex) {
    return 0.0;
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Scaffold(
        backgroundColor: const Color(0xFF1A202C),
        appBar: AppBar(
          title: const Text(
            'Tutorial',
            style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
          ),
          backgroundColor: const Color(0xFF2D3748),
          elevation: 0,
          iconTheme: const IconThemeData(color: Colors.white),
        ),
        body: const Center(
          child: CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation<Color>(Color(0xFF10B981)),
          ),
        ),
      );
    }

    return Scaffold(
      backgroundColor: const Color(0xFF1A202C),
      appBar: AppBar(
        title: const Text(
          'Tutorial',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
        backgroundColor: const Color(0xFF2D3748),
        elevation: 0,
        iconTheme: const IconThemeData(color: Colors.white),
        actions: [
          TextButton(
            onPressed: () async {
              final shouldSkip = await showDialog<bool>(
                context: context,
                builder: (context) => AlertDialog(
                  title: const Text('Skip Tutorial?'),
                  content: const Text(
                    'The Vegas Walk Method is complicated, and most people find the tutorial helps them understand it. Are you sure you want to skip it?',
                  ),
                  actions: [
                    TextButton(
                      onPressed: () => Navigator.of(context).pop(false),
                      child: const Text('Cancel'),
                    ),
                    TextButton(
                      onPressed: () => Navigator.of(context).pop(true),
                      child: const Text('Skip'),
                    ),
                  ],
                ),
              );
              if (shouldSkip == true) {
                final prefs = await SharedPreferences.getInstance();
                await prefs.setBool('hasSeenTutorial', true);
                if (widget.fromStartWalk) {
                  Navigator.of(context).pushAndRemoveUntil(
                    MaterialPageRoute(
                      builder: (context) =>
                          const SetupRunPage(fromSplash: true),
                    ),
                    (route) => false,
                  );
                } else {
                  Navigator.of(context).pushAndRemoveUntil(
                    MaterialPageRoute(
                      builder: (context) => const MainScaffold(initialTab: 0),
                    ),
                    (route) => false,
                  );
                }
              }
            },
            child: const Text(
              'Skip',
              style: TextStyle(
                color: Colors.white,
                fontSize: 16,
                fontWeight: FontWeight.w500,
              ),
            ),
          ),
        ],
      ),
      body: Column(
        children: [
          // Video placeholder section
          Container(
            margin: const EdgeInsets.all(16),
            child: AspectRatio(
              aspectRatio: 16 / 9,
              child: Container(
                decoration: BoxDecoration(
                  color: const Color(0xFF4A5568),
                  borderRadius: BorderRadius.circular(12),
                  border: Border.all(color: Colors.grey[600]!, width: 1),
                ),
                child: Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Icon(
                        Icons.play_circle_outline,
                        size: 64,
                        color: Colors.grey[400],
                      ),
                      const SizedBox(height: 16),
                      Text(
                        'Video Tutorial',
                        style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                          color: Colors.grey[300],
                        ),
                      ),
                      const SizedBox(height: 8),
                      Text(
                        'Video content coming soon',
                        style: TextStyle(fontSize: 14, color: Colors.grey[400]),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ),

          // Slideshow section
          Expanded(
            child: Container(
              margin: const EdgeInsets.fromLTRB(16, 0, 16, 32),
              decoration: BoxDecoration(
                color: const Color(0xFF2D3748),
                borderRadius: BorderRadius.circular(12),
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withOpacity(0.2),
                    blurRadius: 8,
                    offset: const Offset(0, 2),
                  ),
                ],
              ),
              child: Column(
                children: [
                  // Slide content
                  Expanded(
                    child: Stack(
                      children: [
                        GestureDetector(
                          onHorizontalDragEnd: (details) {
                            if (details.primaryVelocity! > 0) {
                              // Swipe right - go to previous slide
                              if (_currentSlideIndex > 0) {
                                _goToSlide(_currentSlideIndex - 1);
                              }
                            } else if (details.primaryVelocity! < 0) {
                              // Swipe left - go to next slide
                              if (_currentSlideIndex < _slides.length - 1) {
                                _goToSlide(_currentSlideIndex + 1);
                              }
                            }
                          },
                          child: Container(
                            padding: const EdgeInsets.fromLTRB(8, 16, 8, 0),
                            child: Markdown(
                              controller: _scrollController,
                              data: _slides[_currentSlideIndex].content,
                              styleSheet: MarkdownStyleSheet(
                                h1: const TextStyle(
                                  color: Colors.white,
                                  fontSize: 24,
                                  fontWeight: FontWeight.bold,
                                ),
                                h2: const TextStyle(
                                  color: Colors.white,
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold,
                                ),
                                p: const TextStyle(
                                  color: Colors.grey,
                                  fontSize: 20,
                                  height: 1.0,
                                ),
                                pPadding: const EdgeInsets.only(bottom: 14),
                                strong: const TextStyle(
                                  color: Colors.white,
                                  fontWeight: FontWeight.bold,
                                ),
                                listBullet: const TextStyle(color: Colors.grey),
                              ),
                            ),
                          ),
                        ),
                        // Green dot animation
                        Positioned(
                          right: 16,
                          bottom: 16,
                          child: _dotAnimation != null
                              ? AnimatedBuilder(
                                  animation: Listenable.merge([
                                    _dotAnimation!,
                                    _dotOpacityAnimation!,
                                  ]),
                                  builder: (context, child) {
                                    return Opacity(
                                      opacity:
                                          _dotOpacityAnimation?.value ?? 1.0,
                                      child: Transform.translate(
                                        offset: Offset(0, _dotAnimation!.value),
                                        child: Container(
                                          width: 40,
                                          height: 40,
                                          decoration: BoxDecoration(
                                            color: Colors.blue,
                                            shape: BoxShape.circle,
                                            boxShadow: [
                                              BoxShadow(
                                                color: Colors.black.withOpacity(
                                                  0.3,
                                                ),
                                                blurRadius: 8,
                                                offset: const Offset(0, 2),
                                              ),
                                            ],
                                          ),
                                          child: const Icon(
                                            Icons.pan_tool,
                                            color: Colors.white,
                                            size: 20,
                                          ),
                                        ),
                                      ),
                                    );
                                  },
                                )
                              : Container(
                                  width: 40,
                                  height: 40,
                                  decoration: BoxDecoration(
                                    color: Colors.blue,
                                    shape: BoxShape.circle,
                                    boxShadow: [
                                      BoxShadow(
                                        color: Colors.black.withOpacity(0.3),
                                        blurRadius: 8,
                                        offset: const Offset(0, 2),
                                      ),
                                    ],
                                  ),
                                  child: const Icon(
                                    Icons.pan_tool,
                                    color: Colors.white,
                                    size: 20,
                                  ),
                                ),
                        ),
                      ],
                    ),
                  ),

                  // Navigation controls
                  Container(
                    padding: const EdgeInsets.fromLTRB(16, 16, 16, 32),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        // Navigation buttons on the left
                        Row(
                          children: [
                            IconButton(
                              onPressed: _currentSlideIndex > 0
                                  ? () => _goToSlide(_currentSlideIndex - 1)
                                  : null,
                              icon: Icon(
                                Icons.arrow_back_ios,
                                color: _currentSlideIndex > 0
                                    ? Colors.white
                                    : Colors.grey[600],
                                size: 20,
                              ),
                            ),
                            IconButton(
                              onPressed: () async {
                                if (_currentSlideIndex < _slides.length - 1) {
                                  _goToSlide(_currentSlideIndex + 1);
                                } else {
                                  final prefs =
                                      await SharedPreferences.getInstance();
                                  await prefs.setBool('hasSeenTutorial', true);
                                  if (widget.fromStartWalk) {
                                    Navigator.of(context).pushAndRemoveUntil(
                                      MaterialPageRoute(
                                        builder: (context) =>
                                            const SetupRunPage(
                                              fromSplash: true,
                                            ),
                                      ),
                                      (route) => false,
                                    );
                                  } else {
                                    Navigator.of(context).pushAndRemoveUntil(
                                      MaterialPageRoute(
                                        builder: (context) =>
                                            const MainScaffold(initialTab: 0),
                                      ),
                                      (route) => false,
                                    );
                                  }
                                }
                              },
                              icon: Icon(
                                Icons.arrow_forward_ios,
                                color: Colors.white,
                                size: 20,
                              ),
                            ),
                          ],
                        ),
                        // Page count on the right
                        Text(
                          '${_currentSlideIndex + 1} / ${_slides.length}',
                          style: TextStyle(
                            color: Colors.grey[400],
                            fontSize: 14,
                            fontWeight: FontWeight.w500,
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _goToSlide(int index) {
    setState(() {
      _currentSlideIndex = index;
      _videoProgress =
          _slides[index].videoTimestamp / 100.0; // Assuming 100 second video
    });
    // Reset scroll position to top
    _scrollController.animateTo(
      0,
      duration: const Duration(milliseconds: 300),
      curve: Curves.easeInOut,
    );
  }
}

class TutorialSlide {
  final String title;
  final String content;
  final double videoTimestamp;
  final Map<String, dynamic> metadata;

  TutorialSlide({
    required this.title,
    required this.content,
    required this.videoTimestamp,
    this.metadata = const {},
  });
}
